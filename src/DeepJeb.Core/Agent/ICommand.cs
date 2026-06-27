using System;

namespace DeepJeb.Core.Agent
{
    /// <summary>
    /// A user-facing slash command that executes locally (no LLM round-trip).
    /// Commands are registered with the CommandDispatcher at startup.
    /// </summary>
    public interface ICommand
    {
        /// <summary>Command name without leading slash (e.g. "retry").</summary>
        string Name { get; }

        /// <summary>One-line description shown in /help listing.</summary>
        string Description { get; }

        /// <summary>Usage string with arguments hint (e.g. "/retry").</summary>
        string Usage { get; }

        /// <summary>
        /// Execute the command with the given arguments and context.
        /// Returns a result that may include a display message or trigger a send.
        /// </summary>
        CommandResult Execute(string args, CommandContext ctx);
    }

    /// <summary>
    /// Execution context passed to command handlers.
    /// Provides callbacks for interacting with the session, UI, and pipeline.
    /// </summary>
    public class CommandContext
    {
        /// <summary>The current chat session (for accessing Messages, LastUserMessage, etc.).</summary>
        public ChatSession Session { get; set; }

        /// <summary>Called to trigger a new pipeline send (used by /retry).</summary>
        public Action<string> OnSendMessage { get; set; }

        /// <summary>Called to stop an in-progress generation.</summary>
        public Action OnStopGeneration { get; set; }

        /// <summary>Called to display a system message in the chat window.</summary>
        public Action<string> DisplayMessage { get; set; }

        /// <summary>Called to force-save the current session to disk.</summary>
        public Action OnSaveSession { get; set; }

        /// <summary>Called to refresh the session list dropdown.</summary>
        public Action OnRefreshSessionList { get; set; }

        /// <summary>Called to request a redraw of the chat window.</summary>
        public Action OnRequestRedraw { get; set; }

        /// <summary>Called to rebuild the display from Session.Messages. Used by /undo.</summary>
        public Action OnRebuildDisplay { get; set; }
    }

    /// <summary>
    /// Result of command execution returned to the caller (ChatWindow).
    /// </summary>
    public class CommandResult
    {
        /// <summary>Whether the command succeeded.</summary>
        public bool Success { get; set; }

        /// <summary>Feedback message displayed as a System message in chat.</summary>
        public string Message { get; set; }

        /// <summary>
        /// If true, the caller should re-trigger OnSendMessage with SendText.
        /// Used by /retry to resend the last user message.
        /// </summary>
        public bool TriggerSend { get; set; }

        /// <summary>The text to send when TriggerSend is true.</summary>
        public string SendText { get; set; }
    }
}
