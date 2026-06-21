using System;
using System.Collections.Generic;
using System.IO;
using DeepJeb.Core.Json;

namespace DeepJeb.Core.Configuration
{
    /// <summary>
    /// Persists DeepJeb configuration to GameData/DeepJeb/DeepJeb.cfg as JSON.
    /// Stores provider data as raw dictionaries to avoid depending on Protocol layer.
    /// DeepJebMod converts to/from ProviderConfig objects at the Mod level.
    /// </summary>
    public class ConfigurationManager
    {
        private readonly string _configPath;

        /// <summary>Raw provider entries (one dictionary per provider).</summary>
        public List<Dictionary<string, object>> Providers { get; set; } = new List<Dictionary<string, object>>();

        public string ActiveProviderName { get; set; }
        public string ActiveModel { get; set; }
        public string ReasoningEffort { get; set; } = "medium";
        public bool ThinkingEnabled { get; set; }
        public int ThinkingBudgetTokens { get; set; } = 16000;

        /// <summary>Fires when Save() fails (e.g., I/O error).</summary>
        public Action<Exception> OnSaveError { get; set; }

        /// <summary>Model context limits, keyed by model name. Loaded/saved with config.</summary>
        public Dictionary<string, int> ModelContextLimits { get; set; } = new Dictionary<string, int>();

        public ConfigurationManager(string gameDataRoot)
        {
            _configPath = Path.Combine(gameDataRoot, "DeepJeb", "DeepJeb.cfg");
        }

        public bool Load()
        {
            if (!File.Exists(_configPath)) return false;

            try
            {
                var json = File.ReadAllText(_configPath, System.Text.Encoding.UTF8);
                var root = JsonMapper.Parse(json) as Dictionary<string, object>;
                if (root == null) return false;

                if (root.TryGetValue("providers", out object provObj) && provObj is List<object> provList)
                {
                    Providers.Clear();
                    foreach (var item in provList)
                    {
                        if (item is Dictionary<string, object> d)
                            Providers.Add(new Dictionary<string, object>(d));
                    }
                }

                ActiveProviderName = GetStr(root, "active_provider");
                ActiveModel = GetStr(root, "active_model");
                ReasoningEffort = GetStr(root, "reasoning_effort") ?? "medium";
                ThinkingEnabled = GetBool(root, "thinking_enabled");
                ThinkingBudgetTokens = GetInt(root, "thinking_budget_tokens", 16000);

                // Load model context limits
                ModelContextLimits.Clear();
                if (root.TryGetValue("model_context_limits", out object limObj) && limObj is List<object> limList)
                {
                    foreach (var item in limList)
                    {
                        if (item is Dictionary<string, object> ld)
                        {
                            string model = GetStr(ld, "model");
                            int tokens = GetInt(ld, "tokens", 0);
                            if (!string.IsNullOrEmpty(model) && tokens > 0)
                                ModelContextLimits[model] = tokens;
                        }
                    }
                }
                return true;
            }
            catch { return false; }
        }

        public void Save()
        {
            try
            {
                var dir = Path.GetDirectoryName(_configPath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                var provList = new List<object>();
                foreach (var p in Providers)
                    provList.Add(new Dictionary<string, object>(p));

                // Serialize model context limits to list of {model, limit} objects
                var limitList = new List<object>();
                foreach (var kvp in ModelContextLimits)
                {
                    limitList.Add(new Dictionary<string, object>
                    {
                        ["model"] = kvp.Key,
                        ["tokens"] = kvp.Value
                    });
                }

                var root = new Dictionary<string, object>
                {
                    ["providers"] = provList,
                    ["active_provider"] = ActiveProviderName,
                    ["active_model"] = ActiveModel,
                    ["reasoning_effort"] = ReasoningEffort,
                    ["thinking_enabled"] = ThinkingEnabled,
                    ["thinking_budget_tokens"] = ThinkingBudgetTokens,
                    ["model_context_limits"] = limitList
                };

                File.WriteAllText(_configPath, JsonMapper.Stringify(root), System.Text.Encoding.UTF8);
            }
            catch (System.Exception ex)
            {
                OnSaveError?.Invoke(ex);
            }
        }

        private static string GetStr(Dictionary<string, object> d, string k) =>
            d.TryGetValue(k, out object v) && v != null ? v.ToString() : null;

        private static bool GetBool(Dictionary<string, object> d, string k)
        {
            if (d.TryGetValue(k, out object v) && v != null)
            {
                if (v is bool b) return b;
                if (bool.TryParse(v.ToString(), out bool r)) return r;
            }
            return false;
        }

        private static int GetInt(Dictionary<string, object> d, string k, int def)
        {
            if (d.TryGetValue(k, out object v) && v != null)
            {
                if (v is int i) return i;
                if (v is long l) return (int)l;
                if (int.TryParse(v.ToString(), out int r)) return r;
            }
            return def;
        }
    }
}
