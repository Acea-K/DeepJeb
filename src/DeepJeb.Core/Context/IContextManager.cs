using System.Collections.Generic;
using DeepJeb.Core.Models;

namespace DeepJeb.Core.Context
{
    /// <summary>
    /// Manages conversation context limits: token counting,
    /// model capability detection, and auto-truncation.
    /// </summary>
    public interface IContextManager
    {
        /// <summary>Get the cached context limit for a model.</summary>
        int? GetContextLimit(string modelName);

        /// <summary>Set/update the context limit for a model.</summary>
        void SetContextLimit(string modelName, int tokenLimit);

        /// <summary>Estimate the token count of a message list.</summary>
        int EstimateTokenCount(List<ChatMessage> messages);

        /// <summary>
        /// Truncate the message list to fit within the model's limit.
        /// Removes oldest message pairs first; preserves the system prompt.
        /// </summary>
        /// <returns>The truncated list, or the original if under limit.</returns>
        List<ChatMessage> TruncateIfNeeded(List<ChatMessage> messages, string modelName, int reservedTokens = 0);
    }
}
