using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeepJeb.Core.Models;

namespace DeepJeb.Core.Agent
{
    /// <summary>
    /// Holds the state for one chat conversation: message history,
    /// current provider/model, and a reference to the processing pipeline.
    ///
    /// This is the bridge between the Unity UI layer and Core processing.
    /// ChatWindow calls SendAsync; ChatSession runs the pipeline and
    /// returns results that the UI renders.
    /// </summary>
    public class ChatSession
    {
        private readonly ChatPipeline _pipeline;

        /// <summary>All messages in this conversation (system prompt + history).</summary>
        public List<ChatMessage> Messages { get; set; }

        /// <summary>Current AI provider name (e.g. "OpenAI").</summary>
        public string ProviderName { get; set; }

        /// <summary>Current model name (e.g. "gpt-4o").</summary>
        public string ModelName { get; set; }

        /// <summary>Session creation timestamp.</summary>
        public DateTime CreatedAt { get; }

        /// <summary>Session ID (yyyyMMdd-HHmmss format).</summary>
        public string SessionId { get; }

        /// <summary>Whether a generation is currently in progress.</summary>
        public bool IsGenerating { get; set; }

        /// <summary>Latest error message, if any.</summary>
        public string LastError { get; private set; }

        /// <summary>Token usage info from last response.</summary>
        public int LastPromptTokens { get; private set; }
        public int LastCompletionTokens { get; private set; }

        public ChatSession(ChatPipeline pipeline)
        {
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            Messages = new List<ChatMessage>();
            CreatedAt = DateTime.UtcNow;
            SessionId = CreatedAt.ToString("yyyyMMdd-HHmmss") + "-" + Guid.NewGuid().ToString("N").Substring(0, 4);
        }

        /// <summary>
        /// Send a user message and get the AI's response.
        /// Returns the full pipeline result (including updated conversation).
        /// Caller must set Messages = result.UpdatedConversation on the main thread.
        /// </summary>
        public async Task<PipelineResult> SendAsync(
            string userText,
            Func<List<ChatMessage>, List<ITool>, Task<AiResponse>> sendFunc)
        {
            if (string.IsNullOrWhiteSpace(userText))
                return new PipelineResult { Success = false, ErrorMessage = "Empty input" };

            IsGenerating = true;
            LastError = null;

            try
            {
                var result = await _pipeline.ProcessAsync(
                    Messages, userText, sendFunc, ModelName);

                if (!result.Success)
                {
                    LastError = result.ErrorMessage ?? "Blocked by " + (result.BlockedBy ?? "filter");
                    IsGenerating = false;
                    return result;
                }

                IsGenerating = false;
                return result;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                IsGenerating = false;
                return new PipelineResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        private const int MaxMessages = 200;

        /// <summary>Trim old messages if over the limit. Keeps system prompts + recent messages.</summary>
        public void TrimHistory()
        {
            if (Messages == null || Messages.Count <= MaxMessages) return;
            var system = Messages.FindAll(m => m.Role == ChatMessage.RoleType.System);
            var rest = new List<ChatMessage>();
            for (int i = Messages.Count - 1; i >= 0 && rest.Count + system.Count < MaxMessages; i--)
                if (Messages[i].Role != ChatMessage.RoleType.System)
                    rest.Insert(0, Messages[i]);
            Messages.Clear();
            Messages.AddRange(system);
            Messages.AddRange(rest);
        }

        /// <summary>
        /// Clear the conversation, preserving the system prompt if present.
        /// Also resets security filter state (soft keyword counters).
        /// </summary>
        public void Clear()
        {
            Messages.Clear();
            // Inject fresh system prompt at conversation start
            if (!string.IsNullOrEmpty(_pipeline.SystemPrompt))
                Messages.Add(ChatMessage.CreateSystem(_pipeline.SystemPrompt));
            LastError = null;
            _pipeline.ResetSecurity();
        }

        /// <summary>
        /// Create a fresh session with a new ID.
        /// </summary>
        [Obsolete("Use Clear() instead. NewSession() does not update SessionId/CreatedAt " +
            "and will overwrite the previous session file on save.")]
        public void NewSession()
        {
            Messages.Clear();
            LastError = null;
            _pipeline.ResetSecurity();
        }
    }
}
