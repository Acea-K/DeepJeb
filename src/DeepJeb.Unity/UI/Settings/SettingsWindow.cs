using System;
using System.Collections.Generic;
using DeepJeb.Protocol;
using DeepJeb.Protocol.Anthropic;
using DeepJeb.Protocol.Google;
using DeepJeb.Protocol.OpenAI;
using DeepJeb.Unity.Localization;
using UnityEngine;

namespace DeepJeb.Unity.UI.Settings
{
    /// <summary>
    /// Settings window for managing API providers, models, and preferences.
    ///
    /// Layout:
    ///   [Provider list with Remove buttons]
    ///   [Add Provider button → opens wizard]
    ///   [Reasoning Effort / Thinking Mode toggles]
    ///   [Save / Close buttons]
    /// </summary>
    public class SettingsWindow : MonoBehaviour
    {
        public bool IsVisible { get; set; }
        public Rect WindowRect { get; set; } = new Rect(150, 80, 480, 520);
        private bool _centered;

        /// <summary>Currently configured providers.</summary>
        public List<ProviderConfig> Providers { get; set; } = new List<ProviderConfig>();

        /// <summary>Called when providers change (add/remove).</summary>
        public Action OnProvidersChanged { get; set; }

        /// <summary>Called when model limits are fetched (modelName → tokenLimit).</summary>
        public Action<Dictionary<string, int>> OnModelLimitsFetched { get; set; }

        /// <summary>Called to open the add-provider wizard.</summary>
        public Action OnAddProvider { get; set; }

        /// <summary>Current reasoning effort setting.</summary>
        public string ReasoningEffort { get; set; } = "medium";

        /// <summary>Current thinking mode (Anthropic).</summary>
        public bool ThinkingEnabled { get; set; }

        private static readonly Color AccentColor = new Color(0.302f, 0.549f, 0.851f);

        private Vector2 _scrollPos;
        private Vector2 _presetScrollPos;
        private int _removeIndex = -1;
        private bool _showAddForm;
        private string _newName = "";
        private string _newKey = "";
        private string _newUrl = "";
        private int _newProtoIdx;
        private string _connectionStatus = "";
        private List<string> _availableModels = new List<string>();
        private List<string> _enabledModels = new List<string>();
        private bool _testingConnection;

        private async System.Threading.Tasks.Task<List<string>> GetModelsAsync(object client)
        {
            List<ModelInfo> models = null;
            if (client is OpenAiClient oai)
                models = await oai.GetAvailableModelsAsync();
            else if (client is AnthropicClient ant)
                models = await ant.GetAvailableModelsAsync();
            else if (client is GoogleClient goo)
                models = await goo.GetAvailableModelsAsync();

            if (models != null)
            {
                // Store model context limits for persistence
                var limits = new Dictionary<string, int>();
                foreach (var m in models)
                {
                    if (!string.IsNullOrEmpty(m.Id) && m.MaxInputTokens.HasValue && m.MaxInputTokens.Value > 0)
                        limits[m.Id] = m.MaxInputTokens.Value;
                }
                if (limits.Count > 0)
                    OnModelLimitsFetched?.Invoke(limits);

                return models.ConvertAll(m => m.Id);
            }
            return new List<string>();
        }

        private async void TestConnection()
        {
            _testingConnection = true;
            _connectionStatus = "";

            try
            {
                var cfg = new ProviderConfig
                {
                    Name = _newName,
                    Protocol = (ProtocolType)_newProtoIdx,
                    ApiKey = _newKey,
                    BaseUrl = string.IsNullOrWhiteSpace(_newUrl) ? null : _newUrl
                };

                var client = ApiClientFactory.CreateClient(cfg);
                bool ok = false;

                if (client is OpenAiClient oai)
                    ok = await oai.TestConnectionAsync();
                else if (client is AnthropicClient ant)
                    ok = await ant.TestConnectionAsync();
                else if (client is GoogleClient goo)
                    ok = await goo.TestConnectionAsync();

                if (ok)
                {
                    _connectionStatus = "✓ Connected";
                    // Fetch available models
                    try
                    {
                        var models = await GetModelsAsync(client);
                        _availableModels = models ?? new List<string>();
                    }
                    catch { _availableModels = new List<string>(); }
                }
                else
                {
                    _connectionStatus = "✗ Failed";
                }
            }
            catch
            {
                _connectionStatus = "✗ Failed";
            }
            finally
            {
                _testingConnection = false;
            }
        }

        private void OnGUI()
        {
            if (!IsVisible) return;

            if (!_centered)
            {
                WindowRect = new Rect(
                    (Screen.width - WindowRect.width) / 2f,
                    (Screen.height - WindowRect.height) / 2f,
                    WindowRect.width, WindowRect.height);
                _centered = true;
            }

            GUI.skin = HighLogic.Skin;
            WindowRect = GUI.Window(GetInstanceID(), WindowRect, DrawWindow,
                DeepJebLoc.SettingsTitle, HighLogic.Skin.window);
        }

        private void DrawWindow(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, WindowRect.width, 24));

            float y = 28;

            // --- Provider list ---
            GUI.Label(new Rect(8, y, 120, 20), DeepJebLoc.ApiProviders, HighLogic.Skin.label);
            y += 22;

            Rect listRect = new Rect(8, y, WindowRect.width - 16, 160);
            GUI.Box(listRect, "");

            float contentH = Mathf.Max(listRect.height, Providers.Count * 26f + 10f);
            Rect viewRect = new Rect(0, 0, listRect.width - 18, contentH);

            _scrollPos = GUI.BeginScrollView(listRect, _scrollPos, viewRect);

            float lineY = 4;
            for (int i = 0; i < Providers.Count; i++)
            {
                var p = Providers[i];
                GUI.Label(new Rect(4, lineY, viewRect.width - 60, 20),
                    p.Name + " (" + p.Protocol + ")", HighLogic.Skin.label);

                if (GUI.Button(new Rect(viewRect.width - 54, lineY, 50, 20), DeepJebLoc.Remove, HighLogic.Skin.button))
                    _removeIndex = i;

                lineY += 26;
            }

            GUI.EndScrollView();

            // Confirm remove
            if (_removeIndex >= 0 && _removeIndex < Providers.Count)
            {
                Rect confirmRect = new Rect(8, y + 164, WindowRect.width - 16, 28);
                GUI.Box(confirmRect, "");
                GUI.Label(new Rect(12, y + 168, 280, 20),
                    DeepJebLoc.RemoveConfirmPrefix + Providers[_removeIndex].Name + DeepJebLoc.RemoveConfirmSuffix, HighLogic.Skin.label);
                if (GUI.Button(new Rect(300, y + 166, 50, 22), DeepJebLoc.Yes, HighLogic.Skin.button))
                {
                    Providers.RemoveAt(_removeIndex);
                    _removeIndex = -1;
                    OnProvidersChanged?.Invoke();
                }
                if (GUI.Button(new Rect(354, y + 166, 50, 22), DeepJebLoc.No, HighLogic.Skin.button))
                    _removeIndex = -1;
            }

            y += 170 + (_removeIndex >= 0 ? 28 : 0);

            // --- Add Provider ---
            if (!_showAddForm)
            {
                if (GUI.Button(new Rect(8, y, 120, 24), DeepJebLoc.AddProvider, HighLogic.Skin.button))
                    _showAddForm = true;
            }
            else
            {
                float formH = _availableModels.Count > 0 ? 380 : 280;
                GUI.Box(new Rect(8, y, WindowRect.width - 16, formH), "");

                // Left: scrollable preset list
                float presetH = formH - 40f;
                Rect presetRect = new Rect(12, y + 22, 150, presetH);
                GUI.Label(new Rect(12, y + 4, 150, 18), "Presets:", HighLogic.Skin.label);
                GUI.Box(presetRect, "");

                var presets = ProviderConfig.BuiltInProviders();
                float pContentH = (presets.Count + 1) * 22f;
                Rect pViewRect = new Rect(0, 0, presetRect.width - 18, pContentH);
                _presetScrollPos = GUI.BeginScrollView(presetRect, _presetScrollPos, pViewRect);
                float py = 0;
                foreach (var p in presets)
                {
                    if (GUI.Button(new Rect(0, py, pViewRect.width, 20), p.Name, HighLogic.Skin.button))
                    {
                        _newName = p.Name;
                        _newProtoIdx = (int)p.Protocol;
                        _newUrl = p.BaseUrl ?? "";
                        _newKey = "";
                    }
                    py += 22;
                }
                // Custom option
                GUI.color = AccentColor;
                if (GUI.Button(new Rect(0, py, pViewRect.width, 20), "Custom...", HighLogic.Skin.button))
                {
                    _newName = ""; _newProtoIdx = 0; _newUrl = ""; _newKey = "";
                }
                GUI.color = Color.white;
                GUI.EndScrollView();

                // Right: form fields
                float fx = 172;
                float fy = y + 4;
                GUI.Label(new Rect(fx, fy, 60, 18), "Name:", HighLogic.Skin.label);
                _newName = GUI.TextField(new Rect(fx + 55, fy, 220, 18), _newName ?? "", HighLogic.Skin.textField);
                fy += 24;
                GUI.Label(new Rect(fx, fy, 60, 18), "Protocol:", HighLogic.Skin.label);
                string[] protos = { "OpenAI", "Anthropic", "Google" };
                _newProtoIdx = GUI.Toolbar(new Rect(fx + 55, fy, 200, 18), _newProtoIdx, protos, HighLogic.Skin.button);
                fy += 26;
                GUI.Label(new Rect(fx, fy, 60, 18), "API Key:", HighLogic.Skin.label);
                _newKey = GUI.TextField(new Rect(fx + 55, fy, 220, 18), _newKey ?? "", HighLogic.Skin.textField);
                fy += 24;
                GUI.Label(new Rect(fx, fy, 60, 18), "Base URL:", HighLogic.Skin.label);
                _newUrl = GUI.TextField(new Rect(fx + 55, fy, 220, 18), _newUrl ?? "", HighLogic.Skin.textField);
                fy += 24;

                // Test Connection button
                GUI.enabled = !_testingConnection && !string.IsNullOrWhiteSpace(_newKey);
                if (GUI.Button(new Rect(fx, fy, 120, 22),
                    _testingConnection ? DeepJebLoc.Testing : DeepJebLoc.TestConnection, HighLogic.Skin.button))
                {
                    TestConnection();
                }
                GUI.enabled = true;
                if (!string.IsNullOrEmpty(_connectionStatus))
                    GUI.Label(new Rect(fx + 115, fy, 160, 22), _connectionStatus, HighLogic.Skin.label);
                fy += 24;

                // Model checkboxes — scrollable to prevent overflow
                if (_availableModels.Count > 0)
                {
                    GUI.Label(new Rect(fx, fy, 100, 18), "Models:", HighLogic.Skin.label);
                    fy += 18;
                    float mcBoxH = Mathf.Min(100f, _availableModels.Count * 18f + 4f);
                    Rect mcRect = new Rect(fx, fy, 230, mcBoxH);
                    GUI.Box(mcRect, "");
                    float mcContentH = _availableModels.Count * 18f + 4f;
                    Rect mcViewRect = new Rect(0, 0, mcRect.width - 18, mcContentH);
                    Vector2 mcScroll = GUI.BeginScrollView(mcRect, Vector2.zero, mcViewRect);
                    float mcY = 2;
                    for (int m = 0; m < _availableModels.Count; m++)
                    {
                        string model = _availableModels[m];
                        bool enabled = _enabledModels.Contains(model);
                        bool newVal = GUI.Toggle(new Rect(4, mcY, mcViewRect.width - 4, 16), enabled, model, HighLogic.Skin.label);
                        if (newVal != enabled)
                        {
                            if (newVal) _enabledModels.Add(model);
                            else _enabledModels.Remove(model);
                        }
                        mcY += 18;
                    }
                    GUI.EndScrollView();
                    fy += mcBoxH + 4;
                }

                if (GUI.Button(new Rect(fx, fy, 60, 22), DeepJebLoc.Save, HighLogic.Skin.button))
                {
                    if (!string.IsNullOrWhiteSpace(_newName) && !string.IsNullOrWhiteSpace(_newKey))
                    {
                        Providers.Add(new ProviderConfig
                        {
                            Name = _newName.Trim(),
                            Protocol = (ProtocolType)_newProtoIdx,
                            ApiKey = _newKey.Trim(),
                            BaseUrl = string.IsNullOrWhiteSpace(_newUrl) ? null : _newUrl.Trim(),
                            EnabledModels = new List<string>(_enabledModels)
                        });
                        OnProvidersChanged?.Invoke();
                        _newName = ""; _newKey = ""; _newUrl = ""; _newProtoIdx = 0;
                        _connectionStatus = ""; _availableModels.Clear(); _enabledModels.Clear();
                        _showAddForm = false;
                    }
                }
                if (GUI.Button(new Rect(fx + 68, fy, 60, 22), DeepJebLoc.Close, HighLogic.Skin.button))
                {
                    _newName = ""; _newKey = ""; _newUrl = ""; _newProtoIdx = 0;
                    _connectionStatus = ""; _availableModels.Clear(); _enabledModels.Clear();
                    _showAddForm = false;
                }
                y += formH;
            }

            y += 32;

            // --- Reasoning Effort ---
            GUI.Label(new Rect(8, y, 140, 20), DeepJebLoc.ReasoningEffort, HighLogic.Skin.label);
            string[] levels = { "low", "medium", "high", "xhigh", "max" };
            int currentIdx = Array.IndexOf(levels, ReasoningEffort ?? "medium");
            if (currentIdx < 0) currentIdx = 1;

            int newIdx = GUI.Toolbar(new Rect(150, y, 280, 20), currentIdx, levels, HighLogic.Skin.button);
            if (newIdx != currentIdx)
                ReasoningEffort = levels[newIdx];

            y += 28;

            // --- Thinking Mode ---
            GUI.Label(new Rect(8, y, 140, 20), DeepJebLoc.ThinkingMode, HighLogic.Skin.label);
            bool newThinking = GUI.Toggle(new Rect(150, y, 200, 20), ThinkingEnabled, DeepJebLoc.ThinkingLabel);
            if (newThinking != ThinkingEnabled)
                ThinkingEnabled = newThinking;

            y += 30;

            // --- Save / Close ---
            if (GUI.Button(new Rect(8, y, 80, 26), DeepJebLoc.Save, HighLogic.Skin.button))
            {
                OnProvidersChanged?.Invoke();
                IsVisible = false;
            }

            if (GUI.Button(new Rect(WindowRect.width - 88, y, 80, 26), DeepJebLoc.Close, HighLogic.Skin.button))
                IsVisible = false;
        }
    }
}
