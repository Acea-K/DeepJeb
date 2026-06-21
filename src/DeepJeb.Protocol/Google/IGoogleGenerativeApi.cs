using System.Collections.Generic;
using System.Threading.Tasks;
using DeepJeb.Core.Models;

namespace DeepJeb.Protocol.Google
{
    /// <summary>
    /// Abstraction over the Google Gemini / Generative Language API.
    /// Google's native protocol differs from both OpenAI and Anthropic
    /// in authentication, content format, and function-calling syntax.
    /// </summary>
    public interface IGoogleGenerativeApi
    {
        string ProviderName { get; }

        /// <summary>Fetch available models.</summary>
        Task<List<ModelInfo>> GetAvailableModelsAsync();

        /// <summary>
        /// Generate content (non-streaming). Uses Google's native
        /// generateContent endpoint with functionDeclarations.
        /// </summary>
        Task<ChatResponse> GenerateContentAsync(
            List<ChatMessage> messages,
            string model,
            List<Core.Agent.ITool> tools = null);

        /// <summary>
        /// Generate content with streaming. The callback receives
        /// text chunks as they arrive.
        /// </summary>
        Task GenerateContentStreamingAsync(
            List<ChatMessage> messages,
            string model,
            List<Core.Agent.ITool> tools,
            System.Action<string> onToken,
            System.Action onComplete,
            System.Action<string> onError,
            System.Action<System.Net.HttpWebRequest> onRequestCreated = null);

        Task<bool> TestConnectionAsync();
    }
}
