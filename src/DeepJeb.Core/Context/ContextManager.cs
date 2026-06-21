using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using DeepJeb.Core.Models;

namespace DeepJeb.Core.Context
{
    /// <summary>
    /// Manages conversation context: token estimation, model limit caching,
    /// and automatic truncation when approaching the model's context window.
    /// </summary>
    public class ContextManager : IContextManager
    {
        // Known model context limits (characters). Used as fallback when
        // the API doesn't provide a limit.
        private static readonly Dictionary<string, int> KnownModelLimits = new Dictionary<string, int>
        {
            // Anthropic
            { "claude", 200000 },
            { "claude-sonnet", 200000 },
            { "claude-sonnet-4-6", 200000 },
            { "claude-opus", 200000 },
            { "claude-opus-4-8", 200000 },
            { "claude-haiku", 200000 },
            { "claude-haiku-4-5", 200000 },

            // OpenAI
            { "gpt-4o", 128000 },
            { "gpt-4-turbo", 128000 },
            { "gpt-4", 8192 },
            { "gpt-3.5-turbo", 16385 },

            // DeepSeek
            { "deepseek", 1000000 },
            { "deepseek-chat", 1000000 },
            { "deepseek-reasoner", 1000000 },

            // Google Gemini
            { "gemini", 32000 },
            { "gemini-2.5", 1000000 },
            { "gemini-1.5", 1000000 },

            // Mistral
            { "mistral", 128000 },
            { "mixtral", 32000 },
        };

        // Cached limits: model name (lowercased) → token limit
        private readonly ConcurrentDictionary<string, int> _contextLimits;

        // Characters-per-token heuristic
        private const double CharsPerToken = 4.0;

        // Truncation threshold: 90% of context limit
        private const double TruncationThreshold = 0.9;

        public ContextManager()
        {
            _contextLimits = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            // Pre-seed with known limits
            foreach (var kvp in KnownModelLimits)
            {
                _contextLimits[kvp.Key] = kvp.Value;
            }
        }

        /// <summary>
        /// Get the cached context limit for a model. Returns null if unknown.
        /// Tries partial match first, then falls back to substring match.
        /// </summary>
        public int? GetContextLimit(string modelName)
        {
            if (string.IsNullOrEmpty(modelName))
                return null;

            // Exact match
            if (_contextLimits.TryGetValue(modelName, out int limit))
                return limit;

            // Substring match (e.g. "gpt-4o-2024-05-13" matches "gpt-4o").
            // Sort by key length descending so longer (more specific) keys win.
            foreach (var kvp in _contextLimits.OrderByDescending(k => k.Key.Length))
            {
                if (modelName.IndexOf(kvp.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                    return kvp.Value;
            }

            return null;
        }

        /// <summary>
        /// Set/update the context limit for a model (from API response).
        /// </summary>
        public void SetContextLimit(string modelName, int tokenLimit)
        {
            if (string.IsNullOrEmpty(modelName))
                return;

            _contextLimits[modelName.ToLowerInvariant()] = tokenLimit;
        }

        /// <summary>
        /// Estimate the token count of a list of messages.
        /// Uses a character-based heuristic: total_chars / 4.
        /// </summary>
        public int EstimateTokenCount(List<ChatMessage> messages)
        {
            if (messages == null || messages.Count == 0)
                return 0;

            int totalChars = 0;
            foreach (var msg in messages)
            {
                if (!string.IsNullOrEmpty(msg.Content))
                    totalChars += msg.Content.Length;

                // Include tool call arguments in estimate
                if (msg.ToolCalls != null)
                {
                    foreach (var tc in msg.ToolCalls)
                    {
                        if (!string.IsNullOrEmpty(tc.Arguments))
                            totalChars += tc.Arguments.Length;
                    }
                }
            }

            return (int)Math.Ceiling(totalChars / CharsPerToken);
        }

        /// <summary>
        /// Truncate the message list if it exceeds 90% of the model's context limit.
        /// Preserves the system prompt (first message if role is System).
        /// Removes the oldest non-system message pairs (user + assistant).
        /// Returns the original list if no truncation is needed or if the limit is unknown.
        /// </summary>
        public List<ChatMessage> TruncateIfNeeded(List<ChatMessage> messages, string modelName, int reservedTokens = 0)
        {
            if (messages == null || messages.Count <= 2)
                return messages;

            int? limit = GetContextLimit(modelName);
            if (limit == null)
                return messages; // Unknown limit — don't truncate

            int estimatedTokens = EstimateTokenCount(messages);
            int maxTokens = Math.Max(1, (int)(limit.Value * TruncationThreshold) - reservedTokens);

            if (estimatedTokens <= maxTokens)
                return messages; // Under threshold — no truncation needed

            // Identify system prompt (first System message)
            int systemIndex = -1;
            for (int i = 0; i < messages.Count; i++)
            {
                if (messages[i].Role == ChatMessage.RoleType.System)
                {
                    systemIndex = i;
                    break;
                }
            }

            // Build truncated list: keep system prompt, then remove oldest pairs
            var result = new List<ChatMessage>();

            // Always preserve the system prompt
            if (systemIndex >= 0)
            {
                result.Add(messages[systemIndex]);
            }

            // Start from the end (newest) and work backward, but we need
            // to keep the conversation coherent. Strategy: keep the last N
            // user/assistant pairs that fit within the limit.
            // Simpler approach: keep system + remove oldest non-system messages
            // until we're under the limit.

            int startIndex = systemIndex >= 0 ? systemIndex + 1 : 0;
            int currentTokens = systemIndex >= 0 ? EstimateTokenCount(new List<ChatMessage> { messages[systemIndex] }) : 0;

            // Collect messages in reverse (newest first) to truncate from the front
            var remaining = new List<ChatMessage>();
            for (int i = messages.Count - 1; i >= startIndex; i--)
            {
                remaining.Add(messages[i]);
            }

            var kept = new List<ChatMessage>();
            foreach (var msg in remaining)
            {
                int msgTokens = EstimateTokenCount(new List<ChatMessage> { msg });
                if (currentTokens + msgTokens <= maxTokens)
                {
                    kept.Add(msg);
                    currentTokens += msgTokens;
                }
                else
                {
                    break; // Can't fit more — stop keeping
                }
            }

            // Reverse kept back to chronological order
            kept.Reverse();
            result.AddRange(kept);

            return result;
        }

        /// <summary>
        /// Check if a model's context limit is known.
        /// </summary>
        public bool HasKnownLimit(string modelName)
        {
            return GetContextLimit(modelName) != null;
        }
    }
}
