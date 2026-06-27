using System.Text;

namespace DeepJeb.Core.Agent.Commands
{
    /// <summary>
    /// /session — Shows current session information (provider, model, message count, session ID).
    /// </summary>
    public class SessionInfoCommand : ICommand
    {
        public string Name => "session";
        public string Description => "Show current session information.";
        public string Usage => "/session";

        public CommandResult Execute(string args, CommandContext ctx)
        {
            if (ctx.Session == null)
                return new CommandResult { Success = false, Message = CommandMessages.NoActiveSession };

            var s = ctx.Session;
            var sb = new StringBuilder();
            sb.AppendLine("Session: " + s.SessionId);
            sb.AppendLine("Created: " + s.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss") + " UTC");
            sb.AppendLine("Provider: " + (string.IsNullOrEmpty(s.ProviderName) ? "(none)" : s.ProviderName));
            sb.AppendLine("Model: " + (string.IsNullOrEmpty(s.ModelName) ? "(none)" : s.ModelName));
            sb.AppendLine("Messages: " + (s.Messages?.Count ?? 0));

            return new CommandResult { Success = true, Message = sb.ToString().TrimEnd() };
        }
    }
}
