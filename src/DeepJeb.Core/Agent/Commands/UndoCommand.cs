using DeepJeb.Core.Models;

namespace DeepJeb.Core.Agent.Commands
{
    /// <summary>
    /// /undo — Removes the last user+assistant message exchange pair
    /// from the session, including all related tool messages and multi-round
    /// tool call chains that follow the last user message.
    /// </summary>
    public class UndoCommand : ICommand
    {
        public string Name => "undo";
        public string Description => "Remove the last user+AI exchange pair from the session.";
        public string Usage => "/undo";

        public CommandResult Execute(string args, CommandContext ctx)
        {
            if (ctx.Session == null)
                return new CommandResult { Success = false, Message = CommandMessages.NoActiveSession };

            var msgs = ctx.Session.Messages;
            if (msgs == null || msgs.Count == 0)
                return new CommandResult { Success = false, Message = CommandMessages.SessionEmpty };

            // Find the last User message — the boundary of the most recent exchange.
            // Remove that User message and everything after it (Assistant, Tool,
            // multi-round tool calls, etc.). This handles all exchange shapes:
            //   User → Assistant  (simple)
            //   User → Assistant[tool_calls] → Tool → Assistant  (single round)
            //   User → Assistant[tool_calls] → Tool → … → Assistant  (multi-round)
            int lastUserIdx = -1;
            for (int i = msgs.Count - 1; i >= 0; i--)
            {
                if (msgs[i].Role == ChatMessage.RoleType.User)
                {
                    lastUserIdx = i;
                    break;
                }
            }

            if (lastUserIdx < 0)
                return new CommandResult { Success = false, Message = CommandMessages.NoUserMessageToUndo };

            // Keep the System prompt(s) at the head (they belong to the session, not the exchange)
            int removeCount = msgs.Count - lastUserIdx;
            msgs.RemoveRange(lastUserIdx, removeCount);

            // Update retry target — scan backward for the last remaining User message
            ctx.Session.LastUserMessage = null;
            for (int i = msgs.Count - 1; i >= 0; i--)
            {
                if (msgs[i].Role == ChatMessage.RoleType.User)
                {
                    ctx.Session.LastUserMessage = msgs[i].Content;
                    break;
                }
            }

            // Persist and update UI
            ctx.OnSaveSession?.Invoke();
            ctx.OnRefreshSessionList?.Invoke();
            ctx.OnRebuildDisplay?.Invoke();

            return new CommandResult
            {
                Success = true,
                Message = string.Format(CommandMessages.UndoRemoved, removeCount)
            };
        }
    }
}
