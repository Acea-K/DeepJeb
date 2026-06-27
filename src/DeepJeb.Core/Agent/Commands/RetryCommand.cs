namespace DeepJeb.Core.Agent.Commands
{
    /// <summary>
    /// /retry — Resends the last user message via the pipeline.
    /// </summary>
    public class RetryCommand : ICommand
    {
        public string Name => "retry";
        public string Description => "Resend the last user message to the AI.";
        public string Usage => "/retry";

        public CommandResult Execute(string args, CommandContext ctx)
        {
            if (ctx.Session == null)
                return new CommandResult { Success = false, Message = CommandMessages.NoActiveSession };

            string lastUser = ctx.Session.LastUserMessage;
            if (string.IsNullOrEmpty(lastUser))
                return new CommandResult { Success = false, Message = CommandMessages.NoMessageToRetry };

            var result = new CommandResult
            {
                Success = true,
                Message = CommandMessages.Retrying,
                TriggerSend = true,
                SendText = lastUser
            };

            return result;
        }
    }
}
