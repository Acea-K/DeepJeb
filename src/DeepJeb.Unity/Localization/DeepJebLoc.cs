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

        // Errors
        public static string NoProviderError => Localizer.Format("#deepjeb_no_provider_error");
        public static string BlockedPrefix => Localizer.Format("#deepjeb_blocked_prefix");
    }
}
