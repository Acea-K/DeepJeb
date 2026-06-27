using System.Linq;
using System.Text;

namespace DeepJeb.Core.Agent.Commands
{
    /// <summary>
    /// /help [command] — Lists all available commands or shows help for a specific one.
    /// </summary>
    public class HelpCommand : ICommand
    {
        public string Name => "help";
        public string Description => "Show this help message.";
        public string Usage => "/help [command]";

        /// <summary>
        /// Reference to the dispatcher, set after registration so the command
        /// can enumerate other commands. Set by the caller after construction.
        /// </summary>
        public CommandDispatcher Dispatcher { get; set; }

        public CommandResult Execute(string args, CommandContext ctx)
        {
            if (Dispatcher == null)
                return new CommandResult { Success = false, Message = CommandMessages.HelpNotInitialized };

            string targetCmd = (args ?? "").Trim().ToLowerInvariant();
            if (!string.IsNullOrEmpty(targetCmd))
            {
                // Show detail for a specific command
                var cmd = Dispatcher.Get(targetCmd);
                if (cmd == null)
                    return new CommandResult
                    {
                        Success = false,
                        Message = string.Format(CommandMessages.UnknownCommand, targetCmd)
                    };

                var sb = new StringBuilder();
                sb.AppendLine(cmd.Name + " — " + cmd.Description);
                sb.AppendLine("  Usage: " + cmd.Usage);
                return new CommandResult { Success = true, Message = sb.ToString().TrimEnd() };
            }

            // List all commands
            var all = Dispatcher.GetAll().OrderBy(c => c.Name);
            var listing = new StringBuilder();
            listing.AppendLine("Available commands:");
            foreach (var cmd in all)
            {
                listing.AppendLine("  " + cmd.Usage + " — " + cmd.Description);
            }
            return new CommandResult { Success = true, Message = listing.ToString().TrimEnd() };
        }
    }
}
