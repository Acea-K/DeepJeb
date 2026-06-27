using System.Collections.Generic;

namespace DeepJeb.Core.Agent
{
    /// <summary>
    /// Registry and dispatcher for slash commands.
    /// Commands are registered at startup; Dispatch() resolves and executes
    /// without an LLM round-trip.
    /// </summary>
    public class CommandDispatcher
    {
        private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        public void Register(ICommand command)
        {
            _commands[command.Name] = command;
        }

        public void Unregister(string name)
        {
            _commands.Remove(name);
        }

        public ICommand Get(string name)
        {
            _commands.TryGetValue(name, out var cmd);
            return cmd;
        }

        public IEnumerable<ICommand> GetAll()
        {
            return _commands.Values;
        }

        public bool HasCommand(string name)
        {
            return _commands.ContainsKey(name);
        }

        /// <summary>
        /// Dispatch a command by name. Strips leading "/" if present.
        /// </summary>
        public CommandResult Dispatch(string name, string args, CommandContext ctx)
        {
            if (string.IsNullOrEmpty(name))
                return new CommandResult { Success = false, Message = CommandMessages.NoCommandSpecified };

            // Strip leading "/" if the caller included it
            if (name.StartsWith("/"))
                name = name.Substring(1);

            name = name.ToLowerInvariant();

            var cmd = Get(name);
            if (cmd == null)
                return new CommandResult
                {
                    Success = false,
                    Message = string.Format(CommandMessages.UnknownCommand, name)
                };

            return cmd.Execute(args ?? "", ctx);
        }
    }
}
