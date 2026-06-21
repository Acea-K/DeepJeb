using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DeepJeb.Core.Agent;
using DeepJeb.Core.Json;
using DeepJeb.Core.Models;

namespace DeepJeb.Protocol.Anthropic
{
    /// <summary>
    /// Anthropic Messages API client.
    /// Protocol: POST /v1/messages. Uses embedded MiniJSON for all JSON.
    /// </summary>
    public class AnthropicClient : IAnthropicCompatibleApi
    {
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public string ProviderName { get; }

        /// <summary>Tool calls captured during the last streaming call.</summary>
        public List<ToolCall> LastStreamingToolCalls { get; private set; }

        private static readonly Dictionary<string, string> AnthropicHeaders = new Dictionary<string, string>
        {
            ["anthropic-version"] = "2023-06-01"
        };

        public AnthropicClient(string providerName, string apiKey, string baseUrl)
        {
            ProviderName = providerName ?? throw new ArgumentNullException(nameof(providerName));
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _baseUrl = (baseUrl ?? "https://api.anthropic.com").TrimEnd('/');
        }

        public async Task<List<ModelInfo>> GetAvailableModelsAsync()
        {
            string url = _baseUrl + "/v1/models";
            string json = await HttpHelper.GetAsync(url, _apiKey, extraHeaders: AnthropicHeaders);
            var root = JsonMapper.Parse(json) as Dictionary<string, object>;
            var result = new List<ModelInfo>();

            if (root != null && root.TryGetValue("data", out object dataObj) && dataObj is List<object> dataList)
            {
                foreach (var item in dataList)
                {
                    if (item is Dictionary<string, object> d)
                    {
                        result.Add(new ModelInfo
                        {
                            Id = GetS(d, "id"),
                            MaxInputTokens = GetI(d, "context_window")
                        });
                    }
                }
            }
            return result;
        }

        public async Task<ChatResponse> SendMessageAsync(
            List<ChatMessage> messages, string model,
            List<ITool> tools = null, bool thinking = false, int thinkingBudget = 16000)
        {
            string url = _baseUrl + "/v1/messages";
            string body = JsonMapper.Stringify(BuildBody(messages, model, tools, false, thinking, thinkingBudget));
            string json = await HttpHelper.PostAsync(url, body, _apiKey, extraHeaders: AnthropicHeaders);
            return ParseResponse(JsonMapper.Parse(json) as Dictionary<string, object>);
        }

        public async Task SendMessageStreamingAsync(
            List<ChatMessage> messages, string model, List<ITool> tools,
            Action<string> onToken, Action onComplete, Action<string> onError,
            bool thinking = false, int thinkingBudget = 16000,
            Action<HttpWebRequest> onRequestCreated = null)
        {
            string url = _baseUrl + "/v1/messages";
            string body = JsonMapper.Stringify(BuildBody(messages, model, tools, true, thinking, thinkingBudget));

            await Task.Run(() =>
            {
                var pendingTools = new Dictionary<int, ToolCallAccumulator>();

                HttpHelper.PostStreaming(url, body, _apiKey,
                    onLine: line =>
                    {
                        if (string.IsNullOrEmpty(line) || !line.StartsWith("data: ")) return;
                        string data = line.Substring(6);
                        if (data == "[DONE]") return;
                        try
                        {
                            var chunk = JsonMapper.Parse(data) as Dictionary<string, object>;
                            if (chunk == null) return;
                            string type = GetS(chunk, "type");
                            int idx = GetI(chunk, "index");

                            if (type == "content_block_start")
                            {
                                var block = GetD(chunk, "content_block");
                                if (block != null && GetS(block, "type") == "tool_use")
                                    pendingTools[idx] = new ToolCallAccumulator
                                    {
                                        Id = GetS(block, "id") ?? Guid.NewGuid().ToString("N"),
                                        Name = GetS(block, "name") ?? "unknown"
                                    };
                            }
                            else if (type == "content_block_delta")
                            {
                                var delta = GetD(chunk, "delta");
                                if (delta == null) return;
                                if (GetS(delta, "type") == "text_delta")
                                {
                                    string text = GetS(delta, "text");
                                    if (!string.IsNullOrEmpty(text)) onToken(text);
                                }
                                else if (GetS(delta, "type") == "input_json_delta" &&
                                         pendingTools.TryGetValue(idx, out var acc))
                                {
                                    acc.InputJson.Append(GetS(delta, "partial_json") ?? "");
                                }
                            }
                            else if (type == "error")
                            {
                                string errMsg = "Stream error";
                                var errObj = chunk.TryGetValue("error", out object eo) ? eo : null;
                                if (errObj is string s) errMsg = s;
                                else if (errObj is Dictionary<string, object> ed)
                                    errMsg = GetS(ed, "message") ?? "Stream error";
                                onError(errMsg);
                                return;
                            }
                        }
                        catch { }
                    },
                    onComplete: () =>
                    {
                        // Capture tool calls from pending accumulators before firing callback
                        if (pendingTools.Count > 0)
                        {
                            LastStreamingToolCalls = pendingTools.Select(kvp =>
                                new ToolCall
                                {
                                    Id = kvp.Value.Id,
                                    Name = kvp.Value.Name,
                                    Arguments = kvp.Value.InputJson.ToString()
                                }).ToList();
                        }
                        else { LastStreamingToolCalls = null; }
                        onComplete();
                    },
                    onError: onError,
                    extraHeaders: AnthropicHeaders,
                    onRequestCreated: onRequestCreated);
            });
        }

        public async Task<bool> TestConnectionAsync()
        {
            string url = _baseUrl + "/v1/models";
            return await HttpHelper.GetAsync(url, _apiKey, 10000, AnthropicHeaders).ContinueWith(t => !t.IsFaulted);
        }

        // ---- Request ----

        private Dictionary<string, object> BuildBody(
            List<ChatMessage> messages, string model,
            List<ITool> tools, bool stream, bool thinking, int thinkingBudget)
        {
            var body = new Dictionary<string, object>
            {
                ["model"] = model,
                ["max_tokens"] = 8192,
                ["stream"] = stream
            };

            // Extract system messages
            var systemMsgs = messages.Where(m => m.Role == ChatMessage.RoleType.System).ToList();
            var otherMsgs = messages.Where(m => m.Role != ChatMessage.RoleType.System).ToList();

            if (systemMsgs.Count == 1)
                body["system"] = systemMsgs[0].Content;
            else if (systemMsgs.Count > 1)
            {
                var sysArr = new List<object>();
                foreach (var sm in systemMsgs)
                    sysArr.Add(new Dictionary<string, object> { ["type"] = "text", ["text"] = sm.Content });
                body["system"] = sysArr;
            }

            body["messages"] = BuildMessages(otherMsgs);

            if (tools != null && tools.Count > 0)
            {
                var tl = new List<object>();
                foreach (var t in tools)
                {
                    tl.Add(new Dictionary<string, object>
                    {
                        ["name"] = t.Name,
                        ["description"] = t.Description,
                        ["input_schema"] = JsonMapper.Parse(t.ParametersSchema)
                    });
                }
                body["tools"] = tl;
            }

            if (thinking)
            {
                body["thinking"] = new Dictionary<string, object>
                {
                    ["type"] = "enabled",
                    ["budget_tokens"] = thinkingBudget
                };
            }

            return body;
        }

        private List<object> BuildMessages(List<ChatMessage> messages)
        {
            var arr = new List<object>();
            foreach (var m in messages)
            {
                var obj = new Dictionary<string, object> { ["role"] = RoleStr(m.Role) };
                var content = new List<object>();

                // Only include text block if content is non-empty (Anthropic rejects empty text blocks)
                if (!string.IsNullOrEmpty(m.Content))
                    content.Add(new Dictionary<string, object> { ["type"] = "text", ["text"] = m.Content });

                if (m.ToolCalls != null && m.ToolCalls.Count > 0)
                {
                    foreach (var tc in m.ToolCalls)
                    {
                        content.Add(new Dictionary<string, object>
                        {
                            ["type"] = "tool_use",
                            ["id"] = tc.Id,
                            ["name"] = tc.Name,
                            ["input"] = JsonMapper.Parse(tc.Arguments)
                        });
                    }
                }

                if (m.Role == ChatMessage.RoleType.Tool)
                {
                    obj["content"] = new List<object>
                    {
                        new Dictionary<string, object>
                        {
                            ["type"] = "tool_result",
                            ["tool_use_id"] = m.ToolCallId,
                            ["content"] = m.Content ?? ""
                        }
                    };
                }
                else
                {
                    // Ensure at least one content block (Anthropic rejects empty arrays)
                    if (content.Count == 0)
                        content.Add(new Dictionary<string, object> { ["type"] = "text", ["text"] = "" });
                    obj["content"] = content;
                }

                arr.Add(obj);
            }
            return arr;
        }

        // ---- Response ----

        private ChatResponse ParseResponse(Dictionary<string, object> root)
        {
            if (root == null) return new ChatResponse { Content = "" };

            string text = "";
            var toolCalls = new List<ToolCall>();
            var blocks = GetL(root, "content");

            foreach (var block in blocks)
            {
                if (block is Dictionary<string, object> b)
                {
                    string type = GetS(b, "type");
                    if (type == "text") text += GetS(b, "text") ?? "";
                    else if (type == "tool_use")
                    {
                        var input = b.TryGetValue("input", out object inp) ? inp : null;
                        toolCalls.Add(new ToolCall
                        {
                            Id = GetS(b, "id") ?? Guid.NewGuid().ToString("N"),
                            Name = GetS(b, "name") ?? "unknown",
                            Arguments = input != null ? JsonMapper.Stringify(input) : "{}"
                        });
                    }
                }
            }

            string stopReason = GetS(root, "stop_reason") ?? "end_turn";
            var usage = GetD(root, "usage");

            return new ChatResponse
            {
                Content = text,
                ToolCalls = toolCalls.Count > 0 ? toolCalls : null,
                FinishReason = stopReason == "end_turn" ? "stop" : stopReason,
                PromptTokens = GetI(usage, "input_tokens"),
                CompletionTokens = GetI(usage, "output_tokens")
            };
        }

        private class ToolCallAccumulator
        {
            public string Id;
            public string Name;
            public System.Text.StringBuilder InputJson = new System.Text.StringBuilder();
        }

        private static string RoleStr(ChatMessage.RoleType r)
        {
            switch (r)
            {
                case ChatMessage.RoleType.User: return "user";
                case ChatMessage.RoleType.Assistant: return "assistant";
                default: return "user";
            }
        }

        // ---- Tree helpers ----
        private static string GetS(Dictionary<string, object> d, string k)
        {
            if (d != null && d.TryGetValue(k, out object v) && v != null) return v.ToString();
            return null;
        }
        private static int GetI(Dictionary<string, object> d, string k)
        {
            if (d != null && d.TryGetValue(k, out object v) && v != null)
            {
                if (v is int i) return i;
                if (v is long l) return (int)l;
                if (v is double f) return (int)f;
            }
            return 0;
        }
        private static Dictionary<string, object> GetD(object item, string k)
        {
            if (item is Dictionary<string, object> d && d.TryGetValue(k, out object v))
                return v as Dictionary<string, object>;
            return null;
        }
        private static List<object> GetL(Dictionary<string, object> d, string k)
        {
            if (d != null && d.TryGetValue(k, out object v) && v is List<object> l) return l;
            return new List<object>();
        }
    }
}
