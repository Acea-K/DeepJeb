namespace DeepJeb.Core.Agent
{
    /// <summary>
    /// User-facing command error/status messages.
    /// Defaults are English; DeepJebMod sets localized overrides at init.
    /// </summary>
    public static class CommandMessages
    {
        // ---- Shared ----
        public static string NoActiveSession = "No active session.";
        public static string NoCommandSpecified = "No command specified.";
        public static string UnknownCommand = "Unknown command '{0}'. Type /help for available commands.";

        // ---- /retry ----
        public static string NoMessageToRetry = "No message to retry.";
        public static string Retrying = "Retrying last message...";

        // ---- /undo ----
        public static string SessionEmpty = "Session is empty. Nothing to undo.";
        public static string NoUserMessageToUndo = "No user message found to undo.";
        public static string UndoRemoved = "Removed {0} message(s). Last exchange undone.";

        // ---- /help ----
        public static string HelpNotInitialized = "Help system not initialized.";

        // ---- /game ----
        public static string GameStateUnavailable = "Game state is not available.";
        public static string NoGameState = "No game state available. Try again in a flight or editor scene.";
        public static string GameStateParseFailed = "Failed to parse game state: {0}";
    }
}
