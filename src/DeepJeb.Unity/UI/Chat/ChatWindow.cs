using System;
using System.Collections.Generic;
using DeepJeb.Core.Agent;
using DeepJeb.Core.Models;
using DeepJeb.Protocol;
using DeepJeb.Unity.Localization;
using DeepJeb.Unity.Rendering;
using System.Threading.Tasks;
using UnityEngine;

namespace DeepJeb.Unity.UI.Chat
{
    /// <summary>
    /// Main draggable chat window (600×500 px default). IMGUI-based for KSP compatibility.
    ///
    /// Layout:
    ///   [Title bar: "DeepJeb AI Assistant" | provider/model | ? help | X close]
    ///   [Message scroll area — auto-follows new messages, manual scroll pauses]
    ///   [Input area: text field + Send/Stop button]
    ///   [Status bar: provider/model | tokens | progress]
    /// </summary>
    public class ChatWindow : MonoBehaviour
    {
        // ---- Public state ----
        public bool IsVisible { get; set; }
        public Rect WindowRect { get; set; } = new Rect(100, 100, 600, 500);
        private bool _centered;

        /// <summary>The chat session driving this window.</summary>
        public ChatSession Session { get; set; }

        /// <summary>Context manager for model limit lookup (set by DeepJebMod).</summary>
        public Core.Context.IContextManager ContextProvider { get; set; }

        /// <summary>Called when user clicks Send. Returns AI response text.</summary>
        public Func<string, string> OnSendMessage { get; set; }

        /// <summary>Current provider name (set by DeepJebMod).</summary>
        public string ProviderName { get; set; }

        /// <summary>Current model name (set by DeepJebMod).</summary>
        public string ModelName { get; set; }

        /// <summary>Called to abort an in-progress generation.</summary>
        public Action OnStopGeneration { get; set; }

        /// <summary>Called when user clicks the Settings button.</summary>
        public Action OnSettingsClicked { get; set; }

        /// <summary>Called when user selects a different provider from the dropdown.</summary>
        public Action<string> OnProviderSelected { get; set; }

        /// <summary>List of provider names for the dropdown.</summary>
        public List<string> Providers { get; set; } = new List<string>();

        /// <summary>Per-provider model lists (index-aligned with Providers).</summary>
        public List<List<string>> ProviderModels { get; set; } = new List<List<string>>();

        /// <summary>List of session IDs for the history dropdown.</summary>
        public List<string> SessionIds { get; set; } = new List<string>();

        /// <summary>Called when user selects a specific model.</summary>
        public Action<string, string> OnModelSelected { get; set; }

        /// <summary>Called when user selects a session from the dropdown.</summary>
        public Action<string> OnSessionSelected { get; set; }

        private bool _showProviderMenu;
        private int _hoveredProvider = -1; // -1 = show providers, >=0 = show models for that provider
        private bool _showSessionMenu;

        // ---- Internal state ----
        private string _inputText = "";
        private Vector2 _scrollPos;
        private bool _showClearConfirm;
        private List<DisplayMessage> _messages = new List<DisplayMessage>();
        private bool _followBottom = true;  // true = auto-scroll to bottom each frame
        private float _prevScrollY;         // detect manual scroll-up
        private DisplayMessage _streamingMsg;    // In-progress streaming assistant message
        private DisplayMessage _toolProgressMsg; // Reusable tool-progress line (Issue 3)
        private DisplayMessage _progressDotsMsg;   // Reference to progress dots message
        private Coroutine _progressDotsRoutine;   // Coroutine reference for clean stop

        // Model fetch — cached from API, keyed by provider name
        private class ModelCacheEntry
        {
            public List<string> Models;
            public DateTime FetchedAt;
        }
        private Dictionary<string, ModelCacheEntry> _modelCache = new Dictionary<string, ModelCacheEntry>();
        private string _fetchingProvider;       // non-null = fetching in progress
        private Task<List<ModelInfo>> _fetchTask;  // pending async fetch task

        /// <summary>Called when ChatWindow needs models for a provider (DeepJebMod handles the fetch).</summary>
        public Action<string> OnFetchModels { get; set; }

        private class DisplayMessage
        {
            public ChatMessage.RoleType Role;
            public string RawText;
            public List<MarkdownParser.MarkdownLine> ParsedLines;
        }

        // Cached GUIStyle for rich text — avoid per-frame allocation in OnGUI
        private GUIStyle _richStyle;
        private GUIStyle RichStyle
        {
            get
            {
                if (_richStyle == null)
                    _richStyle = new GUIStyle(HighLogic.Skin.label) { richText = true };
                return _richStyle;
            }
        }

        // Input lock
        private const string InputLockId = "DeepJeb_ChatInput";
        private bool _inputFocused;

        // Layout constants
        private const float TitleH = 22f;
        private const float ToolbarH = 26f;
        private const float InputH = 58f;
        private const float StatusH = 20f;

        // Colors
        private static readonly Color BgColor = new Color(0.149f, 0.149f, 0.200f);
        private static readonly Color AccentColor = new Color(0.302f, 0.549f, 0.851f);
        private static readonly Color TextColor = new Color(0.851f, 0.851f, 0.902f);
        private static readonly Color ErrorColor = new Color(0.851f, 0.302f, 0.302f);
        private static readonly Color UserColor = new Color(0.302f, 0.549f, 0.851f);
        private static readonly Color AiColor = new Color(0.502f, 0.702f, 0.502f);
        private static readonly Color SystemColor = new Color(0.702f, 0.702f, 0.502f);
        private static readonly Color CodeColor = new Color(0.851f, 0.706f, 0.298f);   // Gold #D9B44D
        private static readonly Color CodeBgColor = new Color(0.12f, 0.12f, 0.16f);

        private void OnDestroy()
        {
            if (_inputFocused)
            {
                InputLockManager.RemoveControlLock(InputLockId);
                _inputFocused = false;
            }
        }

        /// <summary>
        /// Called when the window visibility changes. Releases input lock when hidden
        /// to prevent permanent throttle/staging/camera lockout.
        /// </summary>
        public void OnVisibilityChanged(bool visible)
        {
            if (!visible && _inputFocused)
            {
                InputLockManager.RemoveControlLock(InputLockId);
                _inputFocused = false;
            }
        }

        private void OnGUI()
        {
            if (!IsVisible) return;

            // Center on first open after becoming visible
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
                "DeepJeb", HighLogic.Skin.window);
        }

        private void DrawWindow(int windowId)
        {
            CheckPendingFetch(); // Poll for completed model list fetch

            // --- Title bar: drag area + Help button ---
            GUI.DragWindow(new Rect(0, 0, WindowRect.width - 30, TitleH));
            if (GUI.Button(new Rect(WindowRect.width - 28, 1, 24, TitleH - 2), "?", HighLogic.Skin.button))
                Application.OpenURL("https://github.com/Acea-K/DeepJeb");

            // --- Toolbar: 4 equal buttons between title and messages ---
            float tbY = TitleH + 2;
            float btnW = (WindowRect.width - 16) / 4;
            float btnH = ToolbarH - 4;
            float gap = 4;

            // [1] Model
            string providerLabel = ProviderName ?? (Providers.Count > 0 ? Providers[0] : DeepJebLoc.NoApi);
            if (GUI.Button(new Rect(4, tbY, btnW, btnH), providerLabel, HighLogic.Skin.button))
                _showProviderMenu = !_showProviderMenu;

            // [2] Session
            if (GUI.Button(new Rect(4 + btnW + gap, tbY, btnW, btnH), DeepJebLoc.History, HighLogic.Skin.button))
                _showSessionMenu = !_showSessionMenu;

            // [3] Clear
            if (GUI.Button(new Rect(4 + (btnW + gap) * 2, tbY, btnW, btnH), DeepJebLoc.Clear, HighLogic.Skin.button))
                _showClearConfirm = !_showClearConfirm;

            // [4] Settings
            if (GUI.Button(new Rect(4 + (btnW + gap) * 3, tbY, btnW, btnH), DeepJebLoc.SettingsTitle, HighLogic.Skin.button))
                OnSettingsClicked?.Invoke();

            // Clear confirmation bar
            if (_showClearConfirm)
            {
                float cy = tbY + btnH + 2;
                GUI.Box(new Rect(4, cy, WindowRect.width - 8, 22), "");
                GUI.Label(new Rect(8, cy + 2, 160, 18), DeepJebLoc.ClearConfirm, HighLogic.Skin.label);
                if (GUI.Button(new Rect(WindowRect.width - 120, cy + 1, 50, 20), DeepJebLoc.Yes, HighLogic.Skin.button))
                {
                    Session?.Clear();
                    _messages.Clear();
                    _showClearConfirm = false;
                }
                if (GUI.Button(new Rect(WindowRect.width - 66, cy + 1, 50, 20), DeepJebLoc.No, HighLogic.Skin.button))
                    _showClearConfirm = false;
            }

            float y = tbY + btnH + (_showClearConfirm ? 28 : 4);

            // --- Message area ---
            float msgH = WindowRect.height - y - InputH - StatusH - 6;
            Rect msgRect = new Rect(4, y, WindowRect.width - 8, msgH);

            GUI.Box(msgRect, "");

            // Precise render widths per line type — MUST match the label rects below
            float viewW = msgRect.width - 18;       // = viewRect.width
            float wHeading  = viewW - 20;            // indent 0
            float wText     = viewW - 20 - 12;       // indent 12
            float wList     = viewW - 20 - 16;       // indent 16
            float wOrdered  = viewW - 20 - 20;       // indent 20

            // Calculate total content height — MUST match the rendering pass exactly
            float contentH = 4f; // matches lineY = 4 in rendering
            foreach (var dm in _messages)
            {
                // Role label (matches GUI.Label at height 18)
                bool hasLabel = dm.Role == ChatMessage.RoleType.User ||
                                dm.Role == ChatMessage.RoleType.Assistant ||
                                dm.Role == ChatMessage.RoleType.System;
                if (hasLabel) contentH += 18;
                // Parsed lines
                foreach (var ml in dm.ParsedLines)
                {
                    switch (ml.Type)
                    {
                        case MarkdownParser.LineType.Heading1:
                            contentH += RichStyle.CalcHeight(
                                new GUIContent("<b><size=18>" + ml.RichText + "</size></b>"),
                                wHeading) + 2f; break;
                        case MarkdownParser.LineType.Heading2:
                            contentH += RichStyle.CalcHeight(
                                new GUIContent("<b><size=15>" + ml.RichText + "</size></b>"),
                                wHeading) + 2f; break;
                        case MarkdownParser.LineType.Heading3:
                            contentH += RichStyle.CalcHeight(
                                new GUIContent("<b><size=13>" + ml.RichText + "</size></b>"),
                                wHeading) + 2f; break;
                        case MarkdownParser.LineType.Heading4:
                            contentH += RichStyle.CalcHeight(
                                new GUIContent("<b><size=11>" + ml.RichText + "</size></b>"),
                                wHeading) + 2f; break;
                        case MarkdownParser.LineType.CodeBlock:
                            contentH += CountLines(ml.RichText) * 16f + 12; break;
                        case MarkdownParser.LineType.TableRow:
                            contentH += MaxTableCellHeight(ml, viewW) + 2f; break;
                        case MarkdownParser.LineType.TableSeparator: contentH += 4; break;
                        case MarkdownParser.LineType.ListItem:
                            contentH += RichStyle.CalcHeight(
                                new GUIContent("• " + ml.RichText), wList) + 2f; break;
                        case MarkdownParser.LineType.OrderedItem:
                            contentH += RichStyle.CalcHeight(
                                new GUIContent(ml.RichText), wOrdered) + 2f; break;
                        default:
                            contentH += RichStyle.CalcHeight(
                                new GUIContent(ml.RichText), wText) + 2f;
                            break;
                    }
                }
                contentH += 6; // gap after message
            }
            // CalcHeight underestimates GUI.Label with <size>/<b>/CJK/emoji.
            // Only add margin when content exceeds the viewport (scrollbar is active).
            if (contentH > msgH)
            {
                float margin = Mathf.Min(contentH * 0.10f, 200f);
                contentH += margin;
            }
            contentH = Mathf.Max(contentH, msgH);

            Rect viewRect = new Rect(0, 0, msgRect.width - 18, contentH);
            // Follow-bottom: auto-scroll when new content arrives, stop when
            // user manually scrolls up to read history.
            if (_followBottom)
            {
                _scrollPos.y = Mathf.Max(0, contentH - msgRect.height + 50f);
            }
            _scrollPos = GUI.BeginScrollView(msgRect, _scrollPos, viewRect);
            if (_followBottom && _scrollPos.y < _prevScrollY - 2f)
            {
                _followBottom = false; // User scrolled up — release auto-follow
            }
            _prevScrollY = _scrollPos.y;

            float lineY = 4;
            float lineH = 18;

            foreach (var dm in _messages)
            {
                // Role label
                Color roleColor = dm.Role == ChatMessage.RoleType.User ? UserColor :
                    dm.Role == ChatMessage.RoleType.Assistant ? AiColor :
                    dm.Role == ChatMessage.RoleType.System ? SystemColor : TextColor;

                string roleLabel = dm.Role == ChatMessage.RoleType.User ? DeepJebLoc.RoleYou :
                    dm.Role == ChatMessage.RoleType.Assistant ? DeepJebLoc.RoleAi :
                    dm.Role == ChatMessage.RoleType.System ? DeepJebLoc.RoleSystem : "";
                if (!string.IsNullOrEmpty(roleLabel))
                {
                    // Background strip for role label
                    GUI.color = new Color(roleColor.r, roleColor.g, roleColor.b, 0.15f);
                    GUI.Box(new Rect(2, lineY, viewRect.width - 4, lineH + 2), "");
                    // Role label text
                    GUI.color = roleColor;
                    GUI.Label(new Rect(8, lineY, 60, lineH), "<b>" + roleLabel + "</b>", RichStyle);
                    GUI.color = Color.white;
                    lineY += lineH + 4;
                }

                // Render parsed markdown lines
                foreach (var ml in dm.ParsedLines)
                {
                    float indent = 0;
                    string prefix = "";
                    string text = ml.RichText;

                    switch (ml.Type)
                    {
                        case MarkdownParser.LineType.Heading1:
                            text = "<b><size=18>" + text + "</size></b>";
                            lineH = RichStyle.CalcHeight(new GUIContent(text), wHeading);
                            break;
                        case MarkdownParser.LineType.Heading2:
                            text = "<b><size=15>" + text + "</size></b>";
                            lineH = RichStyle.CalcHeight(new GUIContent(text), wHeading);
                            break;
                        case MarkdownParser.LineType.Heading3:
                            text = "<b><size=13>" + text + "</size></b>";
                            lineH = RichStyle.CalcHeight(new GUIContent(text), wHeading);
                            break;
                        case MarkdownParser.LineType.Heading4:
                            text = "<b><size=11>" + text + "</size></b>";
                            lineH = RichStyle.CalcHeight(new GUIContent(text), wHeading);
                            break;
                        case MarkdownParser.LineType.ListItem:
                            indent = 16; prefix = "• ";
                            lineH = RichStyle.CalcHeight(new GUIContent(prefix + text), wList);
                            break;
                        case MarkdownParser.LineType.OrderedItem:
                            indent = 20;
                            lineH = RichStyle.CalcHeight(new GUIContent(text), wOrdered);
                            break;
                        case MarkdownParser.LineType.CodeBlock:
                            GUI.color = new Color(0.12f, 0.12f, 0.16f);
                            GUI.Box(new Rect(4, lineY, viewRect.width - 8,
                                CountLines(text) * 16f + 8), "");
                            GUI.color = CodeColor;
                            float cbY = lineY + 4;
                            foreach (var codeLine in text.Split('\n'))
                            {
                                GUI.Label(new Rect(12, cbY, viewRect.width - 24, 16),
                                    codeLine.Replace("<", "<​").Replace(">", ">​"), RichStyle);
                                cbY += 16;
                            }
                            GUI.color = Color.white;
                            lineY += CountLines(text) * 16f + 12;
                            continue;
                        case MarkdownParser.LineType.TableRow:
                            RenderTableRow(ml, ref lineY, viewRect.width, RichStyle);
                            continue;
                        case MarkdownParser.LineType.TableSeparator:
                            lineY += 4;
                            continue;
                        default:
                            indent = 12;
                            lineH = 18;
                            break;
                    }

                    if (ml.Type == MarkdownParser.LineType.Text)
                        lineH = RichStyle.CalcHeight(new GUIContent(text), wText);

                    GUI.color = Color.white;
                    GUI.Label(new Rect(8 + indent, lineY, viewRect.width - 20 - indent, lineH + 2),
                        prefix + text, RichStyle);
                    lineY += lineH + 2;
                }
                lineY += 6; // gap between messages
            }
            GUI.color = Color.white;
            GUI.EndScrollView();

            y += msgH + 2;

            // --- Input area ---
            Rect inputRect = new Rect(4, y, WindowRect.width - 70, InputH);
            bool overInput = Event.current != null && inputRect.Contains(Event.current.mousePosition);

            // Input lock management
            if (overInput && !_inputFocused)
            {
                InputLockManager.SetControlLock(
                    ControlTypes.THROTTLE | ControlTypes.STAGING | ControlTypes.CAMERACONTROLS,
                    InputLockId);
                _inputFocused = true;
            }
            else if (!overInput && _inputFocused)
            {
                InputLockManager.RemoveControlLock(InputLockId);
                _inputFocused = false;
            }

            // Enter = send, Ctrl+Enter = newline. Checked BEFORE TextArea.
            if (Event.current != null && Event.current.type == EventType.KeyDown &&
                (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter) &&
                !Event.current.control &&
                GUI.GetNameOfFocusedControl() == "DeepJeb_Input")
            {
                SubmitMessage();
                Event.current.Use();
            }

            GUI.SetNextControlName("DeepJeb_Input");
            _inputText = GUI.TextArea(inputRect, _inputText, 10000, HighLogic.Skin.textArea);

            // Send / Stop button
            Rect btnRect = new Rect(WindowRect.width - 62, y, 58, InputH);
            bool generating = Session != null && Session.IsGenerating;

            if (generating)
            {
                if (GUI.Button(btnRect, DeepJebLoc.Stop, HighLogic.Skin.button))
                    OnStopGeneration?.Invoke();
            }
            else
            {
                if (GUI.Button(btnRect, DeepJebLoc.Send, HighLogic.Skin.button))
                    SubmitMessage();
            }

            y += InputH + 2;

            // --- Status bar ---
            Rect statusRect = new Rect(4, y, WindowRect.width - 8, StatusH);
            string provider = Session?.ProviderName ?? DeepJebLoc.NoApi;
            string modelName = ModelName ?? Session?.ModelName ?? "";
            string contextUsage = GetContextUsage();
            string genStatus = generating ? " " + DeepJebLoc.Generating : "";

            GUI.Label(statusRect,
                provider + " / " + modelName + " | " + contextUsage + genStatus,
                HighLogic.Skin.label);

            // --- Dropdown overlays (rendered last to stay on top) ---
            if (_showProviderMenu)
            {
                float my = tbY + btnH;
                if (_hoveredProvider < 0)
                {
                    // Provider list
                    for (int i = 0; i < Providers.Count; i++)
                    {
                        if (GUI.Button(new Rect(4, my, btnW, 22), Providers[i] + " ▸", HighLogic.Skin.button))
                        {
                            string pn = Providers[i];
                            bool isCached = _modelCache.TryGetValue(pn, out var entry);
                            bool expired = isCached && (DateTime.UtcNow - entry.FetchedAt).TotalMinutes > 15;
                            // Trigger fetch if not cached, expired, and not already fetching
                            if ((!isCached || expired) && _fetchingProvider == null)
                            {
                                _fetchingProvider = pn;
                                OnFetchModels?.Invoke(pn);
                            }
                            _hoveredProvider = i;
                        }
                        my += 22;
                    }
                }
                else
                {
                    // Model submenu
                    int pi = _hoveredProvider;
                    string provName = pi < Providers.Count ? Providers[pi] : "";
                    // Back button
                    if (GUI.Button(new Rect(4, my, btnW, 22), "← " + provName, HighLogic.Skin.button))
                    { _hoveredProvider = -1; _fetchingProvider = null; }
                    my += 22;

                    // Determine which model list to show
                    List<string> displayModels = null;
                    bool fetching = _fetchingProvider == provName;
                    if (_modelCache.TryGetValue(provName, out var cached) && cached.Models.Count > 0)
                        displayModels = cached.Models;
                    else if (!fetching && pi < ProviderModels.Count && ProviderModels[pi] != null && ProviderModels[pi].Count > 0)
                        displayModels = ProviderModels[pi]; // fallback to enabled models

                    if (fetching)
                    {
                        GUI.Label(new Rect(12, my, btnW - 8, 22),
                            "<i>Loading...</i>", HighLogic.Skin.label);
                    }
                    else if (displayModels != null && displayModels.Count > 0)
                    {
                        foreach (var model in displayModels)
                        {
                            bool isCurrent = model == ModelName;
                            if (isCurrent)
                            {
                                GUI.color = new Color(0.302f, 0.549f, 0.851f, 0.4f);
                                GUI.Box(new Rect(12, my, btnW - 8, 22), "");
                                GUI.color = Color.white;
                            }
                            if (GUI.Button(new Rect(12, my, btnW - 8, 22),
                                (isCurrent ? "✓ " : "") + model, HighLogic.Skin.button))
                            {
                                OnModelSelected?.Invoke(provName, model);
                                _showProviderMenu = false;
                                _hoveredProvider = -1;
                                _fetchingProvider = null;
                            }
                            my += 22;
                        }
                    }
                    else
                    {
                        GUI.Label(new Rect(12, my, btnW - 8, 22),
                            "<i>No models available</i>", HighLogic.Skin.label);
                    }
                }
            }

            if (_showSessionMenu)
            {
                float my = tbY + btnH;
                for (int i = 0; i < SessionIds.Count; i++)
                {
                    if (GUI.Button(new Rect(4 + btnW + gap, my, btnW * 2, 22), SessionIds[i], HighLogic.Skin.button))
                    {
                        OnSessionSelected?.Invoke(SessionIds[i]);
                        _showSessionMenu = false;
                    }
                    my += 22;
                }
                if (SessionIds.Count == 0)
                    GUI.Label(new Rect(4 + btnW + gap, my, btnW * 2, 20), DeepJebLoc.NoSessions, HighLogic.Skin.label);
            }
        }

        private void SubmitMessage()
        {
            if (string.IsNullOrWhiteSpace(_inputText)) return;
            string text = _inputText.Trim();
            _inputText = "";

            _messages.Add(new DisplayMessage
            {
                Role = ChatMessage.RoleType.User,
                RawText = text,
                ParsedLines = new List<MarkdownParser.MarkdownLine>
                {
                    new MarkdownParser.MarkdownLine { RichText = text }
                }
            });
            _followBottom = true;

            if (OnSendMessage != null)
            {
                // Start progress dot animation — runs until first token or stop
                _progressDotsRoutine = StartCoroutine(ProgressDots());

                string response = OnSendMessage(text);

                // Sync response: stop dots and display result immediately
                if (response != null)
                {
                    StopProgressDots();
                    AddAiResponse(response);
                }
                // Async (streaming): dots stay until first token arrives, or user stops generation.
                _followBottom = true;
            }
        }

        /// <summary>Stop and remove the progress dot animation.</summary>
        public void StopProgressDots()
        {
            if (_progressDotsRoutine != null)
            {
                StopCoroutine(_progressDotsRoutine);
                _progressDotsRoutine = null;
            }
            if (_progressDotsMsg != null)
            {
                _messages.Remove(_progressDotsMsg);
                _progressDotsMsg = null;
            }
        }

        /// <summary>Called by DeepJebMod when async model fetch completes.</summary>
        public void SetCachedModels(string providerName, List<string> models)
        {
            if (models != null && models.Count > 0)
                _modelCache[providerName] = new ModelCacheEntry
                { Models = models, FetchedAt = DateTime.UtcNow };
        }

        /// <summary>Poll async model fetch — call once per frame from DrawWindow.</summary>
        private void CheckPendingFetch()
        {
            if (_fetchTask == null) return;
            if (!_fetchTask.IsCompleted) return;
            try
            {
                if (!_fetchTask.IsFaulted && _fetchTask.Result != null)
                {
                    var result = new List<string>();
                    foreach (var m in _fetchTask.Result)
                        if (!string.IsNullOrEmpty(m.Id))
                            result.Add(m.Id);
                    if (_fetchingProvider != null && result.Count > 0)
                        _modelCache[_fetchingProvider] = new ModelCacheEntry
                        { Models = result, FetchedAt = DateTime.UtcNow };
                }
            }
            catch { /* fetch failed — leave cache empty */ }
            _fetchTask = null;
            _fetchingProvider = null; // clear regardless of success/failure
        }

        public void SetFetchTask(Task<List<ModelInfo>> task)
        {
            _fetchTask = task;
        }

        private System.Collections.IEnumerator ProgressDots()
        {
            var dots = new[] { ".", "..", "..." };
            var progressMsg = new DisplayMessage
            {
                Role = ChatMessage.RoleType.System,
                RawText = "",
                ParsedLines = new List<MarkdownParser.MarkdownLine>()
            };
            _messages.Add(progressMsg);
            _progressDotsMsg = progressMsg;

            while (true)
            {
                for (int i = 0; i < 3; i++)
                {
                    progressMsg.ParsedLines.Clear();
                    progressMsg.ParsedLines.Add(new MarkdownParser.MarkdownLine
                    {
                        RichText = "<color=#0891B2>" + dots[i] + "</color>"
                    });
                    yield return new WaitForSeconds(0.3f);
                }
            }
        }

        /// <summary>Update raw text during streaming — no markdown parse (avoids flicker).</summary>
        public void UpdatePartialResponse(string partialText)
        {
            if (string.IsNullOrEmpty(partialText)) return;
            if (_streamingMsg == null)
            {
                // First token arrived — stop progress dots, streaming content takes over
                StopProgressDots();
                _streamingMsg = new DisplayMessage
                {
                    Role = ChatMessage.RoleType.Assistant,
                    RawText = "",
                    ParsedLines = new List<MarkdownParser.MarkdownLine>()
                };
                _messages.Add(_streamingMsg);
            }
            _streamingMsg.RawText = partialText;
            _streamingMsg.ParsedLines.Clear();
            _streamingMsg.ParsedLines.Add(new MarkdownParser.MarkdownLine
            {
                Type = MarkdownParser.LineType.Text,
                RichText = MarkdownParser.EscapeRichText(partialText)
            });
            _followBottom = true;
        }

        /// <summary>Finalize current streaming round: full markdown parse, clear tool progress.</summary>
        public void FinalizeStreamingRound()
        {
            if (_toolProgressMsg != null)
            {
                _messages.Remove(_toolProgressMsg);
                _toolProgressMsg = null;
            }
            if (_streamingMsg != null && !string.IsNullOrEmpty(_streamingMsg.RawText))
            {
                _streamingMsg.ParsedLines = MarkdownParser.Parse(_streamingMsg.RawText);
            }
            _streamingMsg = null;
            TrimDisplayMessages();
            _followBottom = true;
        }

        public void AddAiResponse(string text)
        {
            // Clear tool-progress message before showing AI response
            if (_toolProgressMsg != null)
            {
                _messages.Remove(_toolProgressMsg);
                _toolProgressMsg = null;
            }
            if (string.IsNullOrEmpty(text)) return;
            _messages.Add(new DisplayMessage
            {
                Role = ChatMessage.RoleType.Assistant,
                RawText = text,
                ParsedLines = MarkdownParser.Parse(text)
            });
            TrimDisplayMessages();
        }

        private void TrimDisplayMessages()
        {
            const int max = 300;
            if (_messages.Count <= max) return;
            // Split into system and non-system; keep all system messages +
            // the most recent non-system messages up to max total.
            var systemMsgs = new List<DisplayMessage>();
            var nonSystemMsgs = new List<DisplayMessage>();
            foreach (var m in _messages)
            {
                if (m.Role == ChatMessage.RoleType.System)
                    systemMsgs.Add(m);
                else
                    nonSystemMsgs.Add(m);
            }
            int keepNonSystem = max - systemMsgs.Count;
            if (keepNonSystem <= 0) keepNonSystem = 50;
            if (nonSystemMsgs.Count > keepNonSystem)
                nonSystemMsgs = nonSystemMsgs.GetRange(
                    nonSystemMsgs.Count - keepNonSystem, keepNonSystem);
            _messages.Clear();
            _messages.AddRange(systemMsgs);
            _messages.AddRange(nonSystemMsgs);
        }

        /// <summary>
        /// Rebuild the display message list from loaded session messages.
        /// Called after LoadSession to reflect the full conversation history in the UI.
        /// </summary>
        public void RebuildDisplayFromMessages(List<ChatMessage> messages)
        {
            if (messages == null) return;
            _messages.Clear();
            _streamingMsg = null;
            _toolProgressMsg = null;
            foreach (var msg in messages)
            {
                // Never display System or Tool messages — they belong in AI context only.
                if (msg.Role == ChatMessage.RoleType.System) continue;
                if (msg.Role == ChatMessage.RoleType.Tool) continue;
                var displayMsg = new DisplayMessage
                {
                    Role = msg.Role,
                    RawText = msg.Content ?? "",
                    ParsedLines = MarkdownParser.Parse(msg.Content ?? "")
                };
                _messages.Add(displayMsg);
            }
            _followBottom = true;
        }

        /// <summary>Add or update the tool-progress line (reuses single message).</summary>
        public void AddSystemMessage(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            if (_toolProgressMsg == null)
            {
                _toolProgressMsg = new DisplayMessage
                {
                    Role = ChatMessage.RoleType.System,
                    RawText = "",
                    ParsedLines = new List<MarkdownParser.MarkdownLine>()
                };
                _messages.Add(_toolProgressMsg);
            }
            _toolProgressMsg.RawText = text;
            _toolProgressMsg.ParsedLines.Clear();
            _toolProgressMsg.ParsedLines.Add(new MarkdownParser.MarkdownLine
            {
                Type = MarkdownParser.LineType.Text,
                RichText = "<color=#0891B2>" + text + "</color>"
            });
            _followBottom = true;
        }

        private string GetContextUsage()
        {
            if (Session == null || Session.Messages == null)
                return "0 / NO DATA";

            // Count total characters in conversation
            int usedChars = 0;
            foreach (var m in Session.Messages)
            {
                if (!string.IsNullOrEmpty(m.Content))
                    usedChars += m.Content.Length;
                if (m.ToolCalls != null)
                {
                    foreach (var tc in m.ToolCalls)
                        if (!string.IsNullOrEmpty(tc.Arguments))
                            usedChars += tc.Arguments.Length;
                }
            }

            // Estimate tokens (4 chars ≈ 1 token)
            int usedTokens = (int)System.Math.Ceiling(usedChars / 4.0);

            // Get model limit
            string modelName = ModelName ?? Session.ModelName;
            int? limit = ContextProvider?.GetContextLimit(modelName);

            if (limit.HasValue && limit.Value > 0)
            {
                double pct = (double)usedTokens / limit.Value * 100.0;
                return FormatNum(usedTokens) + " / " + FormatNum(limit.Value) +
                       " (" + pct.ToString("F0") + "%)";
            }
            else
            {
                return FormatNum(usedTokens) + " / " + DeepJebLoc.NoData;
            }
        }

        private static string FormatNum(int n)
        {
            if (n >= 1000000)
                return (n / 1000000.0).ToString("F1") + "M";
            if (n >= 1000)
                return (n / 1000.0).ToString("F1") + "K";
            return n.ToString();
        }

        /// <summary>Force a redraw (call after external state changes).</summary>
        public void RequestRedraw()
        {
            _followBottom = true;
        }

        private void RenderTableRow(MarkdownParser.MarkdownLine ml, ref float y, float maxW, GUIStyle style)
        {
            if (ml.TableCells == null || ml.TableCells.Length == 0) return;
            int n = ml.TableCells.Length;
            float cellW = (maxW - 24) / n;
            // Compute max cell height so all cells in row have consistent height
            float rowH = 0;
            for (int i = 0; i < n; i++)
            {
                string cellText = ml.IsTableHeader
                    ? "<b>" + (ml.TableCells[i] ?? "") + "</b>"
                    : (ml.TableCells[i] ?? "");
                float ch = style.CalcHeight(new GUIContent(cellText), cellW);
                if (ch > rowH) rowH = ch;
            }
            if (rowH < 18) rowH = 18;
            for (int i = 0; i < n; i++)
            {
                Rect cr = new Rect(12 + i * cellW, y, cellW, rowH);
                if (ml.IsTableHeader)
                {
                    GUI.color = AccentColor; GUI.Box(cr, ""); GUI.color = Color.white;
                    GUI.Label(cr, "<b>" + (ml.TableCells[i] ?? "") + "</b>", style);
                }
                else
                {
                    if (i % 2 == 0) { GUI.color = new Color(0.18f, 0.18f, 0.22f); GUI.Box(cr, ""); GUI.color = Color.white; }
                    GUI.Label(cr, ml.TableCells[i] ?? "", style);
                }
            }
            y += rowH + 2;
        }

        private static int CountLines(string text)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            int c = 1; foreach (char ch in text) if (ch == '\n') c++;
            return c;
        }

        /// <summary>Compute max cell height for a table row (shared by measurement + rendering).</summary>
        private static float MaxTableCellHeight(MarkdownParser.MarkdownLine ml, float tableWidth)
        {
            if (ml.TableCells == null || ml.TableCells.Length == 0) return 18;
            int n = ml.TableCells.Length;
            float cellW = (tableWidth - 24) / n;
            float maxH = 0;
            // Use a disposable GUIStyle with richText for CalcHeight
            var style = new GUIStyle(HighLogic.Skin.label) { richText = true };
            for (int i = 0; i < n; i++)
            {
                string cellText = ml.IsTableHeader
                    ? "<b>" + (ml.TableCells[i] ?? "") + "</b>"
                    : (ml.TableCells[i] ?? "");
                float ch = style.CalcHeight(new GUIContent(cellText), cellW);
                if (ch > maxH) maxH = ch;
            }
            return maxH < 18 ? 18 : maxH;
        }
    }
}
