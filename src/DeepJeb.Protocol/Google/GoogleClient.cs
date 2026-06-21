using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DeepJeb.Core.Agent;
using DeepJeb.Core.Json;
using DeepJeb.Core.Models;

namespace DeepJeb.Protocol.Google
{
    /// <summary>
    /// Google Gemini / Generative Language API client.
    /// Protocol: POST /v1beta/models/{model}:generateContent.
    /// Uses embedded MiniJSON — zero external dependencies.
    /// </summary>
    public class GoogleClient : IGoogleGenerativeApi
    {
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly bool _useQueryParamAuth;

        public string ProviderName { get; }

        /// <summary>Tool calls captured during the last streaming call.</summary>
        public List<ToolCall> LastStreamingToolCalls { get; private set; }

        public GoogleClient(string providerName, string apiKey, string baseUrl, bool useQueryParamAuth = true)
        {
            ProviderName = providerName ?? throw new ArgumentNullException(nameof(providerName));
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _baseUrl = (baseUrl ?? "https://generativelanguage.googleapis.com").TrimEnd('/');
            _useQueryParamAuth = useQueryParamAuth;
        }

        public async Task<List<ModelInfo>> GetAvailableModelsAsync()
        {
            string url = _baseUrl + "/v1beta/models";
            if (_useQueryParamAuth) url += "?key=" + Uri.EscapeDataString(_apiKey);
            string json = await HttpHelper.GetAsync(url, _useQueryParamAuth ? null : _apiKey);
            var root = JsonMapper.Parse(json) as Dictionary<string, object>;
            var result = new List<ModelInfo>();

            if (root != null && root.TryGetValue("models", out object modelsObj) && modelsObj is List<object> modelsList)
            {
                foreach (var item in modelsList)
                {
                    if (item is Dictionary<string, object> d)
                    {
                        string name = GetS(d, "name") ?? "unknown";
                        if (name.StartsWith("models/")) name = name.Substring(7);
                        result.Add(new ModelInfo
                        {
                            Id = name,
                            MaxInputTokens = GetI(d, "inputTokenLimit"),
                            MaxOutputTokens = GetI(d, "outputTokenLimit")
                        });
                    }
                }
            }
            return result;
        }

        public async Task<ChatResponse> GenerateContentAsync(
            List<ChatMessage> messages, string model, List<ITool> tools = null)
        {
            string url = _baseUrl + "/v1beta/models/" + model + ":generateContent";
            if (_useQueryParamAuth) url += "?key=" + Uri.EscapeDataString(_apiKey);
            string body = JsonMapper.Stringify(BuildBody(messages, tools));
            string json = await HttpHelper.PostAsync(url, body, _useQueryParamAuth ? null : _apiKey);
            return ParseResponse(JsonMapper.Parse(json) as Dictionary<string, object>);
        }

        public async Task GenerateContentStreamingAsync(
            List<ChatMessage> messages, string model, List<ITool> tools,
            Action<string> onToken, Action onComplete, Action<string> onError,
            Action<HttpWebRequest> onRequestCreated = null)
        {
            string url = _baseUrl + "/v1beta/models/" + model + ":streamGenerateContent";
            if (_useQueryParamAuth) url += "?key=" + Uri.EscapeDataString(_apiKey) + "&alt=sse";
            else url += "?alt=sse";

            string body = JsonMapper.Stringify(BuildBody(messages, tools));

            await Task.Run(() =>
            {
                var pendingCalls = new List<ToolCall>();

                HttpHelper.PostStreaming(url, body, _useQueryParamAuth ? null : _apiKey,
                    onLine: line =>
                    {
                        if (string.IsNullOrEmpty(line) || !line.StartsWith("data: ")) return;
                        string data = line.Substring(6);
                        if (data == "[DONE]") return;
                        try
                        {
                            var chunk = JsonMapper.Parse(data) as Dictionary<string, object>;
                            if (chunk == null) return;
                            var candidates = GetL(chunk, "candidates");
                            if (candidates.Count == 0) return;
                            var content = GetD(candidates[0], "content");
                            var parts = GetL(content, "parts");
                            foreach (var part in parts)
                            {
                                var pd = part as Dictionary<string, object>;
                                if (pd == null) continue;
                                string text = GetS(pd, "text");
                                if (!string.IsNullOrEmpty(text)) onToken(text);
                                // Function call in streaming
                                var fnCall = GetD(pd, "functionCall");
                                if (fnCall != null)
                                {
                                    pendingCalls.Add(new ToolCall
                                    {
                                        Id = Guid.NewGuid().ToString("N"),
                                        Name = GetS(fnCall, "name") ?? "unknown",
                                        Arguments = fnCall.TryGetValue("args", out object a) && a != null
                                            ? JsonMapper.Stringify(a) : "{}"
                                    });
                                }
                            }
                        }
                        catch { }
                    },
                    onComplete: () =>
                    {
                        LastStreamingToolCalls = pendingCalls.Count > 0 ? pendingCalls : null;
                        onComplete();
                    },
                    onError: onError,
                    onRequestCreated: onRequestCreated);
            });
        }

        public async Task<bool> TestConnectionAsync()
        {
            string url = _baseUrl + "/v1beta/models";
            if (_useQueryParamAuth) url += "?key=" + Uri.EscapeDataString(_apiKey);
            return await HttpHelper.GetAsync(url, _useQueryParamAuth ? null : _apiKey, 10000).ContinueWith(t => !t.IsFaulted);
        }

        // ---- Request ----

        private Dictionary<string, object> BuildBody(List<ChatMessage> messages, List<ITool> tools)
        {
            var body = new Dictionary<string, object>();
            var contents = new List<object>();

            // Collect system messages into a single systemInstruction
            var systemTexts = new List<string>();
            foreach (var m in messages)
            {
                if (m.Role == ChatMessage.RoleType.System)
                {
                    if (!string.IsNullOrEmpty(m.Content))
                        systemTexts.Add(m.Content);
                    continue;
                }

                var parts = new List<object>();

                if (m.Role == ChatMessage.RoleType.Tool && !string.IsNullOrEmpty(m.ToolCallId))
                {
                    parts.Add(new Dictionary<string, object>
                    {
                        ["functionResponse"] = new Dictionary<string, object>
                        {
                            ["name"] = m.ToolName ?? "unknown",
                            ["response"] = new Dictionary<string, object> { ["content"] = m.Content ?? "" }
                        }
                    });
                }
                else
                {
                    if (!string.IsNullOrEmpty(m.Content))
                        parts.Add(new Dictionary<string, object> { ["text"] = m.Content ?? "" });
                    if (m.ToolCalls != null && m.ToolCalls.Count > 0)
                    {
                        foreach (var tc in m.ToolCalls)
                        {
                            parts.Add(new Dictionary<string, object>
                            {
                                ["functionCall"] = new Dictionary<string, object>
                                {
                                    ["name"] = tc.Name,
                                    ["args"] = JsonMapper.Parse(tc.Arguments)
                                }
                            });
                        }
                    }
                }

                contents.Add(new Dictionary<string, object>
                {
                    ["role"] = GoogleRole(m.Role),
                    ["parts"] = parts
                });
            }

            body["contents"] = contents;

            if (systemTexts.Count > 0)
            {
                body["systemInstruction"] = new Dictionary<string, object>
                {
                    ["parts"] = new List<object>
                    {
                        new Dictionary<string, object> { ["text"] = string.Join("\n\n", systemTexts) }
                    }
                };
            }

            if (tools != null && tools.Count > 0)
            {
                var funcDecls = new List<object>();
                foreach (var t in tools)
                {
                    funcDecls.Add(new Dictionary<string, object>
                    {
                        ["name"] = t.Name,
                        ["description"] = t.Description,
                        ["parameters"] = JsonMapper.Parse(t.ParametersSchema)
                    });
                }
                body["tools"] = new List<object>
                {
                    new Dictionary<string, object> { ["functionDeclarations"] = funcDecls }
                };
            }

            return body;
        }

        // ---- Response ----

        private ChatResponse ParseResponse(Dictionary<string, object> root)
        {
            if (root == null) return new ChatResponse { Content = "", FinishReason = "SAFETY" };

            var candidates = GetL(root, "candidates");
            if (candidates.Count == 0) return new ChatResponse { Content = "", FinishReason = "SAFETY" };

            var candidate = candidates[0] as Dictionary<string, object>;
            var content = GetD(candidate, "content");
            var parts = GetL(content, "parts");

            string text = "";
            var toolCalls = new List<ToolCall>();

            foreach (var part in parts)
            {
                if (part is Dictionary<string, object> p)
                {
                    string t = GetS(p, "text");
                    if (!string.IsNullOrEmpty(t)) text += t;

                    if (p.TryGetValue("functionCall", out object fcObj) && fcObj is Dictionary<string, object> fc)
                    {
                        toolCalls.Add(new ToolCall
                        {
                            Id = Guid.NewGuid().ToString("N"),
                            Name = GetS(fc, "name") ?? "unknown",
                            Arguments = fc.TryGetValue("args", out object a) ? JsonMapper.Stringify(a) : "{}"
                        });
                    }
                }
            }

            string reason = GetS(candidate, "finishReason") ?? "STOP";
            var usage = GetD(root, "usageMetadata");

            return new ChatResponse
            {
                Content = text,
                ToolCalls = toolCalls.Count > 0 ? toolCalls : null,
                FinishReason = reason == "STOP" ? "stop" : reason,
                PromptTokens = GetI(usage, "promptTokenCount"),
                CompletionTokens = GetI(usage, "candidatesTokenCount")
            };
        }

        private static string GoogleRole(ChatMessage.RoleType r)
        {
            switch (r)
            {
                case ChatMessage.RoleType.User: return "user";
                case ChatMessage.RoleType.Assistant: return "model";
                case ChatMessage.RoleType.Tool: return "function";
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
