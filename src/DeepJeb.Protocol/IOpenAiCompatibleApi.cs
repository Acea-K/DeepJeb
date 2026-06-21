using System.Collections.Generic;
using System.Threading.Tasks;
using DeepJeb.Core.Models;

namespace DeepJeb.Protocol
{
    /// <summary>
    /// Abstraction over any OpenAI-compatible Chat Completions API.
    /// Implementations vary only by Base URL and API key — the
    /// protocol (POST /v1/chat/completions, tool calls, streaming)
    /// is identical across OpenAI, DeepSeek, OpenRouter, Moonshot, etc.
    /// </summary>
    public interface IOpenAiCompatibleApi
    {
        /// <summary>Human-readable provider name (e.g. "OpenAI", "DeepSeek").</summary>
        string ProviderName { get; }

        /// <summary>Fetch available models from the /models endpoint.</summary>
        Task<List<ModelInfo>> GetAvailableModelsAsync();

        /// <summary>
        /// Send a chat completion request (non-streaming).
        /// Returns the assistant's response with any tool calls.
        /// </summary>
        Task<ChatResponse> SendChatAsync(
            List<ChatMessage> messages,
            string model,
            List<Core.Agent.ITool> tools = null,
            string reasoningEffort = null);

        /// <summary>
        /// Send a chat completion request with SSE streaming.
        /// The callback is invoked for each token/chunk received.
        /// </summary>
        Task SendChatStreamingAsync(
            List<ChatMessage> messages,
            string model,
            List<Core.Agent.ITool> tools,
            System.Action<string> onToken,
            System.Action onComplete,
            System.Action<string> onError,
            string reasoningEffort = null,
            System.Action<System.Net.HttpWebRequest> onRequestCreated = null);

        /// <summary>Test connectivity with the configured API key.</summary>
        Task<bool> TestConnectionAsync();
    }

    public class ModelInfo
    {
        public string Id { get; set; }
        public int? MaxInputTokens { get; set; }
        public int? MaxOutputTokens { get; set; }
    }

    public class ChatResponse
    {
        public string Content { get; set; }
        public List<ToolCall> ToolCalls { get; set; }
        public string FinishReason { get; set; }
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
    }
}
