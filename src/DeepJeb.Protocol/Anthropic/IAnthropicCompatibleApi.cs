using System.Collections.Generic;
using System.Threading.Tasks;
using DeepJeb.Core.Models;

namespace DeepJeb.Protocol.Anthropic
{
    /// <summary>
    /// Abstraction over the Anthropic Messages API.
    /// Handles Anthropic-native features: thinking mode,
    /// tool use (different schema format from OpenAI), etc.
    /// </summary>
    public interface IAnthropicCompatibleApi
    {
        string ProviderName { get; }

        /// <summary>Fetch available models.</summary>
        Task<List<ModelInfo>> GetAvailableModelsAsync();

        /// <summary>
        /// Send a message (non-streaming). Tool use is via Anthropic's
        /// native tool_use content blocks.
        /// </summary>
        Task<ChatResponse> SendMessageAsync(
            List<ChatMessage> messages,
            string model,
            List<Core.Agent.ITool> tools = null,
            bool thinkingEnabled = false,
            int thinkingBudgetTokens = 16000);

        /// <summary>
        /// Send a message with SSE streaming. The callback receives
        /// text deltas and tool_use/tool_result events.
        /// </summary>
        Task SendMessageStreamingAsync(
            List<ChatMessage> messages,
            string model,
            List<Core.Agent.ITool> tools,
            System.Action<string> onToken,
            System.Action onComplete,
            System.Action<string> onError,
            bool thinkingEnabled = false,
            int thinkingBudgetTokens = 16000,
            System.Action<System.Net.HttpWebRequest> onRequestCreated = null);

        Task<bool> TestConnectionAsync();
    }
}
