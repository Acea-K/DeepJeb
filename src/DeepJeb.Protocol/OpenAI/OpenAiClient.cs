using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DeepJeb.Core.Agent;
using DeepJeb.Core.Json;
using DeepJeb.Core.Models;

namespace DeepJeb.Protocol.OpenAI
{
    /// <summary>
    /// OpenAI-compatible Chat Completions API client.
    /// Covers 12+ providers: OpenAI, DeepSeek, OpenRouter, Grok, Mistral, etc.
    /// Uses embedded MiniJSON for all JSON building/parsing — zero external deps.
    /// </summary>
    public class OpenAiClient : IOpenAiCompatibleApi
    {
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public string ProviderName { get; }

        /// <summary>Tool calls captured during the last streaming call.</summary>
        public List<ToolCall> LastStreamingToolCalls { get; private set; }

        public OpenAiClient(string providerName, string apiKey, string baseUrl)
        {
            ProviderName = providerName ?? throw new ArgumentNullException(nameof(providerName));
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _baseUrl = (baseUrl ?? "https://api.openai.com").TrimEnd('/');
        }

        public async Task<List<ModelInfo>> GetAvailableModelsAsync()
        {
            string url = _baseUrl + "/v1/models";
            string json = await HttpHelper.GetAsync(url, _apiKey);
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
                            Id = GetString(d, "id"),
                            MaxInputTokens = GetInt(d, "max_input_tokens"),
                            MaxOutputTokens = GetInt(d, "max_output_tokens")
                        });
                    }
                }
            }

            return result;
        }

        public async Task<ChatResponse> SendChatAsync(
            List<ChatMessage> messages, string model,
            List<ITool> tools = null, string reasoningEffort = null)
        {
            string url = _baseUrl + "/v1/chat/completions";
            string jsonBody = JsonMapper.Stringify(BuildBody(messages, model, tools, false, reasoningEffort));
            string json = await HttpHelper.PostAsync(url, jsonBody, _apiKey);
            return ParseResponse(JsonMapper.Parse(json) as Dictionary<string, object>);
        }

        public async Task SendChatStreamingAsync(
            List<ChatMessage> messages, string model, List<ITool> tools,
            Action<string> onToken, Action onComplete, Action<string> onError,
            string reasoningEffort = null,
            Action<HttpWebRequest> onRequestCreated = null)
        {
            string url = _baseUrl + "/v1/chat/completions";
            string jsonBody = JsonMapper.Stringify(BuildBody(messages, model, tools, true, reasoningEffort));

            await Task.Run(() =>
            {
                var pendingToolCalls = new Dictionary<int, ToolCallAccumulator>();

                HttpHelper.PostStreaming(url, jsonBody, _apiKey,
                    onLine: line =>
                    {
                        if (string.IsNullOrEmpty(line) || !line.StartsWith("data: ")) return;
                        string data = line.Substring(6);
                        if (data == "[DONE]") return;
                        try
                        {
                            var chunk = JsonMapper.Parse(data) as Dictionary<string, object>;
                            if (chunk == null) return;
                            var choices = GetList(chunk, "choices");
                            if (choices.Count == 0) return;
                            var delta = GetDict(choices[0], "delta");
                            if (delta == null) return;

                            // Text token
                            string text = GetString(delta, "content");
                            if (!string.IsNullOrEmpty(text)) onToken(text);

                            // Tool call accumulation
                            if (delta.TryGetValue("tool_calls", out object tcObj) && tcObj is List<object> tcList)
                            {
                                foreach (var tc in tcList)
                                {
                                    if (!(tc is Dictionary<string, object> tcd)) continue;
                                    int idx = tcd.TryGetValue("index", out object idxObj) ? ConvertToInt(idxObj) : 0;
                                    if (!pendingToolCalls.TryGetValue(idx, out var acc))
                                    {
                                        acc = new ToolCallAccumulator();
                                        pendingToolCalls[idx] = acc;
                                    }
                                    string tcId = GetString(tcd, "id");
                                    if (!string.IsNullOrEmpty(tcId)) acc.Id = tcId;
                                    var func = GetDict(tc, "function");
                                    if (func != null)
                                    {
                                        string fnName = GetString(func, "name");
                                        if (!string.IsNullOrEmpty(fnName)) acc.Name = fnName;
                                        string fnArgs = GetString(func, "arguments");
                                        if (!string.IsNullOrEmpty(fnArgs)) acc.ArgumentsBuilder.Append(fnArgs);
                                    }
                                }
                            }
                        }
                        catch { /* skip malformed chunks */ }
                    },
                    onComplete: () =>
                    {
                        if (pendingToolCalls.Count > 0)
                        {
                            LastStreamingToolCalls = pendingToolCalls.Select(kvp =>
                                new ToolCall
                                {
                                    Id = kvp.Value.Id ?? Guid.NewGuid().ToString("N"),
                                    Name = kvp.Value.Name ?? "unknown",
                                    Arguments = kvp.Value.ArgumentsBuilder.ToString()
                                }).ToList();
                        }
                        else { LastStreamingToolCalls = null; }
                        onComplete();
                    },
                    onError: onError,
                    onRequestCreated: onRequestCreated);
            });
        }

        private class ToolCallAccumulator
        {
            public string Id;
            public string Name;
            public System.Text.StringBuilder ArgumentsBuilder = new System.Text.StringBuilder();
        }

        private static int ConvertToInt(object v)
        {
            if (v is int i) return i;
            if (v is long l) return (int)l;
            if (v is double f) return (int)f;
            if (v != null && int.TryParse(v.ToString(), out int p)) return p;
            return 0;
        }

        public async Task<bool> TestConnectionAsync()
        {
            string url = _baseUrl + "/v1/models";
            return await HttpHelper.GetAsync(url, _apiKey, 10000).ContinueWith(t => !t.IsFaulted);
        }

        // ---- Request building with Dictionary/List trees ----

        private Dictionary<string, object> BuildBody(
            List<ChatMessage> messages, string model,
            List<ITool> tools, bool stream, string reasoningEffort)
        {
            var body = new Dictionary<string, object>
            {
                ["model"] = model,
                ["messages"] = BuildMessages(messages),
                ["stream"] = stream
            };

            if (tools != null && tools.Count > 0)
            {
                var toolsList = new List<object>();
                foreach (var t in tools)
                {
                    toolsList.Add(new Dictionary<string, object>
                    {
                        ["type"] = "function",
                        ["function"] = new Dictionary<string, object>
                        {
                            ["name"] = t.Name,
                            ["description"] = t.Description,
                            ["parameters"] = JsonMapper.Parse(t.ParametersSchema)
                        }
                    });
                }
                body["tools"] = toolsList;
            }

            if (!string.IsNullOrEmpty(reasoningEffort))
                body["reasoning_effort"] = reasoningEffort;

            return body;
        }

        private List<object> BuildMessages(List<ChatMessage> messages)
        {
            var list = new List<object>();
            foreach (var m in messages)
            {
                var obj = new Dictionary<string, object>
                {
                    ["role"] = RoleStr(m.Role),
                    ["content"] = m.Content ?? ""
                };

                if (m.Role == ChatMessage.RoleType.Tool)
                    obj["tool_call_id"] = m.ToolCallId;

                if (m.ToolCalls != null && m.ToolCalls.Count > 0)
                {
                    var tcList = new List<object>();
                    foreach (var tc in m.ToolCalls)
                    {
                        tcList.Add(new Dictionary<string, object>
                        {
                            ["id"] = tc.Id,
                            ["type"] = "function",
                            ["function"] = new Dictionary<string, object>
                            {
                                ["name"] = tc.Name,
                                ["arguments"] = tc.Arguments
                            }
                        });
                    }
                    obj["tool_calls"] = tcList;
                }

                list.Add(obj);
            }
            return list;
        }

        // ---- Response parsing ----

        private ChatResponse ParseResponse(Dictionary<string, object> root)
        {
            if (root == null)
                return new ChatResponse { Content = "", FinishReason = "error" };

            var choices = GetList(root, "choices");
            if (choices.Count == 0)
                return new ChatResponse { Content = "" };

            var choice = choices[0] as Dictionary<string, object>;
            var message = GetDict(choice, "message");
            string content = GetString(message, "content") ?? "";
            string finish = GetString(choice, "finish_reason") ?? "stop";

            var toolCalls = new List<ToolCall>();
            if (message != null && message.TryGetValue("tool_calls", out object tcObj) && tcObj is List<object> tcList)
            {
                foreach (var tc in tcList)
                {
                    if (tc is Dictionary<string, object> tcd)
                    {
                        var func = GetDict(tcd, "function");
                        toolCalls.Add(new ToolCall
                        {
                            Id = GetString(tcd, "id") ?? Guid.NewGuid().ToString("N"),
                            Name = func != null ? GetString(func, "name") ?? "unknown" : "unknown",
                            Arguments = func != null ? GetString(func, "arguments") ?? "{}" : "{}"
                        });
                    }
                }
            }

            var usage = GetDict(root, "usage");
            return new ChatResponse
            {
                Content = content,
                ToolCalls = toolCalls.Count > 0 ? toolCalls : null,
                FinishReason = finish,
                PromptTokens = GetInt(usage, "prompt_tokens"),
                CompletionTokens = GetInt(usage, "completion_tokens")
            };
        }

        private static string RoleStr(ChatMessage.RoleType r)
        {
            switch (r)
            {
                case ChatMessage.RoleType.System: return "system";
                case ChatMessage.RoleType.User: return "user";
                case ChatMessage.RoleType.Assistant: return "assistant";
                case ChatMessage.RoleType.Tool: return "tool";
                default: return "user";
            }
        }

        // ---- Tree helpers ----

        private static string GetString(Dictionary<string, object> d, string key)
        {
            if (d != null && d.TryGetValue(key, out object v) && v != null)
                return v.ToString();
            return null;
        }

        private static int GetInt(Dictionary<string, object> d, string key)
        {
            if (d != null && d.TryGetValue(key, out object v) && v != null)
            {
                if (v is int i) return i;
                if (v is long l) return (int)l;
                if (v is double f) return (int)f;
                if (int.TryParse(v.ToString(), out int p)) return p;
            }
            return 0;
        }

        private static Dictionary<string, object> GetDict(object item, string key)
        {
            if (item is Dictionary<string, object> d && d.TryGetValue(key, out object v))
                return v as Dictionary<string, object>;
            return null;
        }

        private static List<object> GetList(Dictionary<string, object> d, string key)
        {
            if (d != null && d.TryGetValue(key, out object v) && v is List<object> l)
                return l;
            return new List<object>();
        }
    }
}
