using System.Collections.Generic;
using System.Threading.Tasks;
using DeepJeb.Core.Models;

namespace DeepJeb.Core.Agent
{
    /// <summary>
    /// Orchestrates the tool-calling conversation loop:
    /// sends user message → receives response → executes tools → repeats.
    /// </summary>
    public interface IAgentLoop
    {
        /// <summary>
        /// Run one complete agent interaction for a user message.
        /// Returns the final assistant response (text or summary after tool calls).
        /// </summary>
        Task<AgentLoopResult> RunAsync(
            List<ChatMessage> conversation,
            string userMessage,
            AgentLoopConfig config = null);
    }

    /// <summary>
    /// Result of an agent loop execution.
    /// </summary>
    public class AgentLoopResult
    {
        /// <summary>Final assistant response text.</summary>
        public string FinalResponse { get; set; }

        /// <summary>Number of tool-call rounds executed.</summary>
        public int RoundsExecuted { get; set; }

        /// <summary>Whether the loop was terminated early (max rounds, repeat detection).</summary>
        public bool WasTerminatedEarly { get; set; }

        /// <summary>Reason for early termination, if any.</summary>
        public string TerminationReason { get; set; }

        /// <summary>The updated conversation with all new messages appended.</summary>
        public List<ChatMessage> UpdatedConversation { get; set; }
    }
}
