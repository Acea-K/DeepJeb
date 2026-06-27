using KSP.Localization;

namespace DeepJeb.Unity.Localization
{
    /// <summary>
    /// Typed accessor for DeepJeb localization strings.
    /// Wraps KSP.Localizer.Format() so all UI code uses compile-time-checked keys.
    ///
    /// Strings are defined in GameData/DeepJeb/Localization/DeepJeb_Locs.cfg
    /// in both en-us and zh-cn.
    /// </summary>
    public static class DeepJebLoc
    {
        // Chat window
        public static string NoApi => Localizer.Format("#deepjeb_no_api");
        public static string History => Localizer.Format("#deepjeb_history");
        public static string NoSessions => Localizer.Format("#deepjeb_no_sessions");
        public static string Clear => Localizer.Format("#deepjeb_clear");
        public static string ClearConfirm => Localizer.Format("#deepjeb_clear_confirm");
        public static string Yes => Localizer.Format("#deepjeb_yes");
        public static string No => Localizer.Format("#deepjeb_no");
        public static string Send => Localizer.Format("#deepjeb_send");
        public static string Stop => Localizer.Format("#deepjeb_stop");
        public static string Welcome => Localizer.Format("#deepjeb_welcome");
        public static string Generating => Localizer.Format("#deepjeb_generating");
        public static string Stopped => Localizer.Format("#deepjeb_stopped");
        public static string ErrorPrefix => Localizer.Format("#deepjeb_error_prefix");
        public static string RequestFailed => Localizer.Format("#deepjeb_request_failed");

        // Settings
        public static string SettingsTitle => Localizer.Format("#deepjeb_settings_title");
        public static string ApiProviders => Localizer.Format("#deepjeb_api_providers");
        public static string Remove => Localizer.Format("#deepjeb_remove");
        public static string RemoveConfirmPrefix => Localizer.Format("#deepjeb_remove_confirm_prefix");
        public static string RemoveConfirmSuffix => Localizer.Format("#deepjeb_remove_confirm_suffix");
        public static string AddProvider => Localizer.Format("#deepjeb_add_provider");
        public static string ReasoningEffort => Localizer.Format("#deepjeb_reasoning_effort");
        public static string ThinkingMode => Localizer.Format("#deepjeb_thinking_mode");
        public static string ThinkingLabel => Localizer.Format("#deepjeb_thinking_label");
        public static string Save => Localizer.Format("#deepjeb_save");
        public static string Close => Localizer.Format("#deepjeb_close");
        public static string TestConnection => Localizer.Format("#deepjeb_test_connection");
        public static string Testing => Localizer.Format("#deepjeb_testing");

        // Role labels
        public static string RoleYou => Localizer.Format("#deepjeb_role_you");
        public static string RoleAi => Localizer.Format("#deepjeb_role_ai");
        public static string RoleSystem => Localizer.Format("#deepjeb_role_system");

        // Status bar
        public static string NoData => Localizer.Format("#deepjeb_no_data");

        // Settings form
        public static string Presets => Localizer.Format("#deepjeb_presets");
        public static string Custom => Localizer.Format("#deepjeb_custom");
        public static string NameLabel => Localizer.Format("#deepjeb_name_label");
        public static string ProtocolLabel => Localizer.Format("#deepjeb_protocol_label");
        public static string ModelsLabel => Localizer.Format("#deepjeb_models_label");
        public static string ConnectionSuccess => Localizer.Format("#deepjeb_connection_success");
        public static string ConnectionFailed => Localizer.Format("#deepjeb_connection_failed");

        // Model dropdown
        public static string Loading => Localizer.Format("#deepjeb_loading");
        public static string NoModels => Localizer.Format("#deepjeb_no_models");

        // Errors
        public static string NoProviderError => Localizer.Format("#deepjeb_no_provider_error");
        public static string BlockedPrefix => Localizer.Format("#deepjeb_blocked_prefix");
        public static string UnknownError => Localizer.Format("#deepjeb_unknown_error");

        // ---- Slash command errors ----
        public static string CmdNoSession => Localizer.Format("#deepjeb_cmd_no_session");
        public static string CmdNoRetry => Localizer.Format("#deepjeb_cmd_no_retry");
        public static string CmdRetrying => Localizer.Format("#deepjeb_cmd_retrying");
        public static string CmdSessionEmpty => Localizer.Format("#deepjeb_cmd_session_empty");
        public static string CmdNoUndo => Localizer.Format("#deepjeb_cmd_no_undo");
        public static string CmdUndone => Localizer.Format("#deepjeb_cmd_undone");
        public static string CmdHelpInit => Localizer.Format("#deepjeb_cmd_help_init");
        public static string CmdUnknown => Localizer.Format("#deepjeb_cmd_unknown");
        public static string CmdNoCommand => Localizer.Format("#deepjeb_cmd_no_command");
        public static string CmdGameUnavail => Localizer.Format("#deepjeb_cmd_game_unavail");
        public static string CmdNoGameState => Localizer.Format("#deepjeb_cmd_no_game_state");
        public static string CmdGameParse => Localizer.Format("#deepjeb_cmd_game_parse");
    }
}
