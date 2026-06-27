using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeepJeb.Core.Agent;
using DeepJeb.Core.Configuration;
using DeepJeb.Core.Context;
using DeepJeb.Core.Models;
using DeepJeb.Core.Security;
using DeepJeb.Core.Session;
using DeepJeb.Core.Skills;
using DeepJeb.Protocol;
using DeepJeb.Protocol.Anthropic;
using DeepJeb.Protocol.Google;
using DeepJeb.Protocol.OpenAI;
using DeepJeb.Unity.Lifecycle;
using DeepJeb.Unity.Localization;
using DeepJeb.Unity.UI.Chat;
using DeepJeb.Unity.UI.Settings;
using DeepJeb.Unity.Toolbar;
using DeepJeb.Core.Agent.Commands;
using DeepJeb.Unity.Tools;
using UnityEngine;

namespace DeepJeb
{
    [KSPAddon(KSPAddon.Startup.AllGameScenes, true)]
    public class DeepJebMod : MonoBehaviour
    {
        public static DeepJebMod Instance { get; private set; }

        // ---- Core ----
        public ToolRegistry ToolRegistry { get; private set; }
        public FilterPipeline SecurityPipeline { get; private set; }
        public KeywordSkillMatcher SkillMatcher { get; private set; }
        public ContextManager ContextMgr { get; private set; }
        public ChatPipeline ChatPipeline { get; private set; }
        public ChatSession ChatSession { get; private set; }
        public CommandDispatcher CommandDispatcher { get; private set; }
        public ConfigurationManager Config { get; private set; }
        private JsonSessionStore _sessionStore;

        // ---- Protocol ----
        private List<ProviderConfig> _providers = new List<ProviderConfig>();
        private object _activeClient;
        private Coroutine _activeSendCoroutine; // Active send coroutine for stop/cancel support
        private string _activeModel;
        private ProtocolType _activeProtocol;
        private volatile System.Net.HttpWebRequest _activeHttpRequest; // Set by streaming callback for Abort on Stop
        private volatile bool _abortRequested;               // Signals the streaming thread to bail early

        // ---- UI ----
        private GameObject _chatObj;
        private ChatWindow _chatWindow;
        private SettingsWindow _settingsWindow;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Debug.Log("[DeepJeb] Bootstrap...");

            InitializeSecurity();
            InitializeSkills();
            InitializeContext();
            InitializeTools();
            InitializePipeline();
            InitializeCommands();
            InitializeConfig();
            InitializeSessionStore();
            InitializeUI();

            Debug.Log("[DeepJeb] Ready. Tools=" + CountTools() + " Skills=" + SkillMatcher.LoadedSkills.Count);
        }

        // ---- Init steps ----

        private void InitializeSecurity()
        {
            SecurityPipeline = new FilterPipeline();
            SecurityPipeline.AddFilter(new HardKeywordFilter());
            SecurityPipeline.AddFilter(new SoftKeywordFilter());
        }

        private void InitializeSkills()
        {
            SkillMatcher = new KeywordSkillMatcher();
            string skillsDir = KSPUtil.ApplicationRootPath + "GameData/DeepJeb/Skills";
            try { SkillMatcher.LoadSkills(skillsDir); }
            catch (Exception e) { Debug.LogWarning("[DeepJeb] Skill load failed: " + e.Message); }
        }

        private void InitializeContext()
        {
            ContextMgr = new ContextManager();
        }

        private void InitializeTools()
        {
            ToolRegistry = new ToolRegistry();
            string root = KSPUtil.ApplicationRootPath + "GameData";
            var sandbox = new PathSandbox(root);
            ToolRegistry.Register(new ReadFileTool(sandbox));
            ToolRegistry.Register(new WriteFileTool(sandbox));
            ToolRegistry.Register(new DeleteFileTool(sandbox));
            ToolRegistry.Register(new ListDirectoryTool(sandbox));
            ToolRegistry.Register(new FileExistsTool(sandbox));
            ToolRegistry.Register(new BackupFileTool(sandbox));
            ToolRegistry.Register(new GetGameStateTool());
        }

        private void InitializePipeline()
        {
            ChatPipeline = new ChatPipeline(SecurityPipeline, SkillMatcher, ContextMgr, ToolRegistry);

            // Wire tool execution progress to ChatWindow
            ChatPipeline.OnToolExecuting = (name, args) =>
            {
                if (_chatWindow != null)
                    _chatWindow.AddSystemMessage("> " + name + " " + Truncate(args, 80));
            };

            ChatSession = new ChatSession(ChatPipeline);
        }

        private static string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s) || s.Length <= max) return s;
            return s.Substring(0, max) + "...";
        }

        private void InitializeCommands()
        {
            // Apply localized error messages to command system
            CommandMessages.NoActiveSession = DeepJebLoc.CmdNoSession;
            CommandMessages.NoMessageToRetry = DeepJebLoc.CmdNoRetry;
            CommandMessages.Retrying = DeepJebLoc.CmdRetrying;
            CommandMessages.SessionEmpty = DeepJebLoc.CmdSessionEmpty;
            CommandMessages.NoUserMessageToUndo = DeepJebLoc.CmdNoUndo;
            CommandMessages.UndoRemoved = DeepJebLoc.CmdUndone;
            CommandMessages.HelpNotInitialized = DeepJebLoc.CmdHelpInit;
            CommandMessages.UnknownCommand = DeepJebLoc.CmdUnknown;
            CommandMessages.NoCommandSpecified = DeepJebLoc.CmdNoCommand;
            CommandMessages.GameStateUnavailable = DeepJebLoc.CmdGameUnavail;
            CommandMessages.NoGameState = DeepJebLoc.CmdNoGameState;
            CommandMessages.GameStateParseFailed = DeepJebLoc.CmdGameParse;

            CommandDispatcher = new CommandDispatcher();
            CommandDispatcher.Register(new RetryCommand());
            CommandDispatcher.Register(new UndoCommand());

            var helpCmd = new HelpCommand { Dispatcher = CommandDispatcher };
            CommandDispatcher.Register(helpCmd);

            CommandDispatcher.Register(new SessionInfoCommand());

            var gameCmd = new GameCommand();
            // GameStateFetcher: calls GetGameStateTool synchronously (KSP main thread is OK)
            gameCmd.GameStateFetcher = () =>
            {
                try
                {
                    var tool = ToolRegistry?.Get("get_game_state");
                    if (tool == null) return null;
                    var task = tool.ExecuteAsync("{}");
                    // GetGameStateTool is fully synchronous (no awaits);
                    // the task is always already completed on return.
                    if (!task.IsCompleted) task.Wait(1000); // defensive timeout
                    return task.IsCompleted && !task.IsFaulted ? task.Result : null;
                }
                catch { return null; }
            };
            CommandDispatcher.Register(gameCmd);
        }

        private void InitializeSessionStore()
        {
            string sessionsDir = KSPUtil.ApplicationRootPath + "GameData/DeepJeb/Sessions";
            _sessionStore = new JsonSessionStore(sessionsDir);
            Debug.Log("[DeepJeb] Session store ready.");
        }

        private void InitializeConfig()
        {
            Config = new ConfigurationManager(KSPUtil.ApplicationRootPath + "GameData");
            Config.OnSaveError = ex => Debug.LogError("[DeepJeb] Config save failed: " + ex.Message);
            Config.Load();

            // Seed ContextManager with persisted model limits
            foreach (var kvp in Config.ModelContextLimits)
                ContextMgr.SetContextLimit(kvp.Key, kvp.Value);

            // Convert raw dicts → ProviderConfig objects
            if (Config.Providers.Count > 0)
            {
                _providers.Clear();
                foreach (var d in Config.Providers)
                {
                    _providers.Add(new ProviderConfig
                    {
                        Name = GetStr(d, "name"),
                        Protocol = (ProtocolType)Enum.Parse(typeof(ProtocolType), GetStr(d, "protocol") ?? "OpenAI"),
                        ApiKey = ApiKeyObfuscator.Deobfuscate(GetStr(d, "api_key") ?? ""),
                        BaseUrl = GetStr(d, "base_url"),
                        ReasoningEffort = GetStr(d, "reasoning_effort"),
                        ThinkingEnabled = GetBool(d, "thinking_enabled"),
                        ThinkingBudgetTokens = GetInt(d, "thinking_budget_tokens", 16000),
                        EnabledModels = GetStrList(d, "enabled_models")
                    });
                }
                SwitchToProviderByName(Config.ActiveProviderName);
            }

            Debug.Log("[DeepJeb] Config loaded. Providers=" + _providers.Count);
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

        private static List<string> GetStrList(Dictionary<string, object> d, string k)
        {
            var r = new List<string>();
            if (d.TryGetValue(k, out object v) && v is List<object> l)
                foreach (var i in l) if (i != null) r.Add(i.ToString());
            return r;
        }

        private void InitializeUI()
        {
            // Toolbar
            var tbObj = new GameObject("DeepJeb_Toolbar");
            tbObj.transform.parent = transform;
            tbObj.AddComponent<ToolbarButton>().OnToggle = ToggleChat;

            // Lifecycle (auto-save triggers)
            var lcObj = new GameObject("DeepJeb_Lifecycle");
            lcObj.transform.parent = transform;
            var lc = lcObj.AddComponent<ModLifecycle>();
            lc.OnSaveRequested = SaveConfig;

            // Settings window (hidden until opened)
            var setObj = new GameObject("DeepJeb_Settings");
            setObj.transform.parent = transform;
            _settingsWindow = setObj.AddComponent<SettingsWindow>();
            _settingsWindow.Providers = _providers;
            _settingsWindow.OnAddProvider = OpenAddWizard;
            _settingsWindow.OnProvidersChanged = OnProvidersUpdated;
            _settingsWindow.OnModelLimitsFetched = limits =>
            {
                foreach (var kvp in limits)
                {
                    ContextMgr.SetContextLimit(kvp.Key, kvp.Value);
                    Config.ModelContextLimits[kvp.Key] = kvp.Value;
                }
                SaveConfig();
            };
        }

        // ---- Chat window ----

        private void ToggleChat()
        {
            if (_chatObj == null)
            {
                _chatObj = new GameObject("DeepJeb_Chat");
                _chatObj.transform.parent = transform;
                _chatWindow = _chatObj.AddComponent<ChatWindow>();
                _chatWindow.Session = ChatSession;
                _chatWindow.CommandDispatcher = CommandDispatcher;
                _chatWindow.OnSessionSave = AutoSaveSession;
                _chatWindow.OnSessionListRefresh = RefreshSessionList;
                _chatWindow.OnRebuildDisplay = () =>
                {
                    _chatWindow.StopProgressDots();
                    _chatWindow.RebuildDisplayFromMessages(ChatSession.Messages);
                };
                _chatWindow.ContextProvider = ContextMgr;

                _chatWindow.OnSendMessage = userText =>
                {
                    if (_activeClient == null)
                        return DeepJebLoc.ErrorPrefix + " " + DeepJebLoc.NoProviderError;
                    if (ChatSession.IsGenerating)
                        return null;

                    // Start coroutine to avoid freezing the main thread
                    _activeSendCoroutine = StartCoroutine(SendCoroutine(userText));
                    return null; // Response arrives asynchronously via AddAiResponse
                };

                _chatWindow.OnStopGeneration = () =>
                {
                    _abortRequested = true;
                    _activeHttpRequest?.Abort();
                    if (_activeSendCoroutine != null)
                        StopCoroutine(_activeSendCoroutine);
                    ChatSession.IsGenerating = false;
                    _chatWindow?.StopProgressDots();
                    _chatWindow?.FinalizeStreamingRound();
                    _chatWindow?.AddAiResponse("[" + DeepJebLoc.RoleSystem + "] " + DeepJebLoc.Stopped);
                };

                _chatWindow.Providers = _providers.ConvertAll(p => p.Name);
                _chatWindow.ProviderModels = _providers.ConvertAll(p => p.EnabledModels ?? new List<string>());
                _chatWindow.ProviderName = _providers.Count > 0 ? _providers[0].Name : null;
                _chatWindow.OnProviderSelected = name => SwitchToProviderByName(name);
                _chatWindow.OnModelSelected = (provider, model) =>
                {
                    SwitchToProviderByName(provider);
                    _activeModel = model;
                    if (_chatWindow != null) _chatWindow.ModelName = model;
                    ChatSession.ModelName = model;
                };
                _chatWindow.OnSettingsClicked = ShowSettings;
                _chatWindow.OnSessionSelected = LoadSession;
                _chatWindow.OnFetchModels = FetchModelsForProvider;
                RefreshSessionList();
                _chatWindow.ModelName = _activeModel;
                ChatSession.ProviderName = _chatWindow.ProviderName;
                ChatSession.ModelName = _activeModel;
            }

            _chatWindow.IsVisible = !_chatWindow.IsVisible;
            _chatWindow.OnVisibilityChanged(_chatWindow.IsVisible);
        }

        public void ShowSettings()
        {
            _settingsWindow.IsVisible = !_settingsWindow.IsVisible;
            _settingsWindow.Providers = _providers;
            SyncProviderLists();
        }

        private void SyncProviderLists()
        {
            if (_chatWindow != null)
            {
                _chatWindow.Providers = _providers.ConvertAll(p => p.Name);
                _chatWindow.ProviderModels = _providers.ConvertAll(p => p.EnabledModels ?? new List<string>());
            }
        }

        // ---- Protocol wiring ----

        /// <summary>
        /// Create a streaming sendFunc. Each AgentLoop call to this func streams tokens
        /// into tokenQueue (with a sentinel on round start). AgentLoop sees a normal
        /// Task&lt;AiResponse&gt; — streaming display is handled by SendCoroutine polling.
        /// </summary>
        private Func<List<ChatMessage>, List<ITool>, Task<AiResponse>> CreateStreamingSendFunc(
            List<string> tokenQueue, object lockObj)
        {
            var client = _activeClient;
            var model = _activeModel;
            var protocol = _activeProtocol;
            var reasoningEffort = _settingsWindow?.ReasoningEffort;
            var thinkingEnabled = _settingsWindow?.ThinkingEnabled ?? false;

            if (client == null) return null;

            return (msgs, tools) =>
            {
                // Sentry marks a new agent-loop round boundary for SendCoroutine
                lock (lockObj) { tokenQueue.Add("\x00ROUND\x00"); }

                var tcs = new TaskCompletionSource<AiResponse>();
                var contentBuilder = new System.Text.StringBuilder();

                Action<string> onToken = token =>
                {
                    contentBuilder.Append(token);
                    lock (lockObj) { tokenQueue.Add(token); }
                };

                Action onComplete = () => { }; // TCS is resolved by WrapStreamingCall after await

                Action<string> onError = err =>
                    tcs.TrySetException(new System.Exception(err));

                WrapStreamingCall(client, protocol, msgs, model, tools,
                    onToken, onComplete, onError, reasoningEffort, thinkingEnabled,
                    tcs, contentBuilder);

                return tcs.Task;
            };
        }

        private async void WrapStreamingCall(object client, ProtocolType protocol,
            List<ChatMessage> msgs, string model, List<ITool> tools,
            Action<string> onToken, Action onComplete, Action<string> onError,
            string reasoningEffort, bool thinkingEnabled,
            TaskCompletionSource<AiResponse> tcs, System.Text.StringBuilder contentBuilder)
        {
            try
            {
                List<ToolCall> capturedToolCalls = null;

                Action<System.Net.HttpWebRequest> onRequestCreated = req =>
                {
                    _activeHttpRequest = req;
                    _abortRequested = false;
                };

                switch (protocol)
                {
                    case ProtocolType.OpenAI:
                        var oai = (OpenAiClient)client;
                        await oai.SendChatStreamingAsync(
                            msgs, model, tools, onToken, onComplete, onError, reasoningEffort,
                            onRequestCreated: onRequestCreated);
                        capturedToolCalls = oai.LastStreamingToolCalls;
                        break;
                    case ProtocolType.Anthropic:
                        var ant = (AnthropicClient)client;
                        await ant.SendMessageStreamingAsync(
                            msgs, model, tools, onToken, onComplete, onError, thinkingEnabled,
                            onRequestCreated: onRequestCreated);
                        capturedToolCalls = ant.LastStreamingToolCalls;
                        break;
                    case ProtocolType.Google:
                        var goo = (GoogleClient)client;
                        await goo.GenerateContentStreamingAsync(
                            msgs, model, tools, onToken, onComplete, onError,
                            onRequestCreated: onRequestCreated);
                        capturedToolCalls = goo.LastStreamingToolCalls;
                        break;
                    default:
                        onError("Unknown protocol: " + protocol);
                        return;
                }

                string content = contentBuilder.ToString();
                bool hasToolCalls = capturedToolCalls != null && capturedToolCalls.Count > 0;
                tcs.TrySetResult(new AiResponse
                {
                    Content = content,
                    ToolCalls = hasToolCalls ? capturedToolCalls : null,
                    FinishReason = hasToolCalls ? "tool_calls" : "stop"
                });
            }
            catch (Exception ex) { tcs.TrySetException(ex); }
        }

        private void OnProvidersUpdated()
        {
            _providers = _settingsWindow.Providers;
            SyncProviderLists();
            SaveConfig();
            if (_providers.Count > 0 && _activeClient == null)
                SwitchToProvider(0);
        }

        public void SaveConfig()
        {
            // Convert ProviderConfig → raw dicts for storage
            Config.Providers.Clear();
            foreach (var p in _providers)
            {
                Config.Providers.Add(new Dictionary<string, object>
                {
                    ["name"] = p.Name,
                    ["protocol"] = p.Protocol.ToString(),
                    ["api_key"] = ApiKeyObfuscator.Obfuscate(p.ApiKey),
                    ["base_url"] = p.BaseUrl ?? "",
                    ["reasoning_effort"] = p.ReasoningEffort,
                    ["thinking_enabled"] = p.ThinkingEnabled,
                    ["thinking_budget_tokens"] = p.ThinkingBudgetTokens,
                    ["enabled_models"] = p.EnabledModels ?? new List<string>()
                });
            }
            Config.ActiveProviderName = ChatSession?.ProviderName;
            Config.ActiveModel = ChatSession?.ModelName;
            Config.ReasoningEffort = _settingsWindow?.ReasoningEffort ?? "medium";
            Config.ThinkingEnabled = _settingsWindow?.ThinkingEnabled ?? false;
            Config.Save();
        }

        private void SwitchToProvider(int index)
        {
            if (index < 0 || index >= _providers.Count) return;
            var cfg = _providers[index];
            _activeClient = ApiClientFactory.CreateClient(cfg);
            _activeProtocol = cfg.Protocol;
            _activeModel = cfg.EnabledModels != null && cfg.EnabledModels.Count > 0
                ? cfg.EnabledModels[0] : null;

            if (_chatWindow != null)
            {
                _chatWindow.ProviderName = cfg.Name;
                _chatWindow.ModelName = _activeModel;
                ChatSession.ProviderName = cfg.Name;
                ChatSession.ModelName = _activeModel;
            }
            Config.ActiveProviderName = cfg.Name;
            Config.ActiveModel = _activeModel;
        }

        private bool SwitchToProviderByName(string name)
        {
            int idx = _providers.FindIndex(p => p.Name == name);
            if (idx >= 0) { SwitchToProvider(idx); return true; }
            return false;
        }

        private void OpenAddWizard()
        {
            // TODO Phase 4: Add Provider wizard (3-step: select provider → enter key → test → save)
            Debug.Log("[DeepJeb] Add Provider wizard requested (not yet implemented)");
        }

        // ---- Helpers ----

        /// <summary>Async fetch available models for a provider and push to ChatWindow cache.</summary>
        private void FetchModelsForProvider(string providerName)
        {
            var cfg = _providers.Find(p => p.Name == providerName);
            if (cfg == null) return;
            var client = ApiClientFactory.CreateClient(cfg);
            try
            {
                switch (cfg.Protocol)
                {
                    case ProtocolType.OpenAI:
                        _chatWindow?.SetFetchTask(((IOpenAiCompatibleApi)client).GetAvailableModelsAsync());
                        break;
                    case ProtocolType.Anthropic:
                        _chatWindow?.SetFetchTask(((IAnthropicCompatibleApi)client).GetAvailableModelsAsync());
                        break;
                    case ProtocolType.Google:
                        _chatWindow?.SetFetchTask(((IGoogleGenerativeApi)client).GetAvailableModelsAsync());
                        break;
                }
            }
            catch { /* fetch failed silently */ }
        }

        private int CountTools()
        {
            int n = 0;
            foreach (var _ in ToolRegistry.GetAll()) n++;
            return n;
        }

        private IEnumerator SendCoroutine(string userText)
        {
            // Token queue + lock for thread-safe streaming
            var tokenQueue = new List<string>();
            var lockObj = new object();
            var task = ChatSession.SendAsync(userText, CreateStreamingSendFunc(tokenQueue, lockObj));

            var sb = new System.Text.StringBuilder();

            // Poll tokens each frame (non-blocking)
            while (!task.IsCompleted)
            {
                List<string> batch = null;
                lock (lockObj)
                {
                    if (tokenQueue.Count > 0)
                    {
                        batch = new List<string>(tokenQueue);
                        tokenQueue.Clear();
                    }
                }
                if (batch != null)
                {
                    foreach (var t in batch)
                    {
                        if (t == "\x00ROUND\x00")
                        {
                            // Flush accumulated text from previous round before finalizing
                            if (sb.Length > 0)
                            {
                                _chatWindow?.UpdatePartialResponse(sb.ToString());
                            }
                            _chatWindow?.FinalizeStreamingRound();
                            sb.Clear();
                        }
                        else
                        {
                            sb.Append(t);
                        }
                    }
                    _chatWindow?.UpdatePartialResponse(sb.ToString());
                }
                yield return null;
            }

            // Drain any tokens that arrived between last frame and onComplete
            List<string> finalBatch = null;
            lock (lockObj)
            {
                if (tokenQueue.Count > 0)
                {
                    finalBatch = new List<string>(tokenQueue);
                    tokenQueue.Clear();
                }
            }
            if (finalBatch != null)
            {
                foreach (var t in finalBatch)
                {
                    if (t == "\x00ROUND\x00")
                    {
                        if (sb.Length > 0)
                        {
                            _chatWindow?.UpdatePartialResponse(sb.ToString());
                        }
                        _chatWindow?.FinalizeStreamingRound();
                        sb.Clear();
                    }
                    else { sb.Append(t); }
                }
                _chatWindow?.UpdatePartialResponse(sb.ToString());
            }

            // Finalize the last streaming round
            _chatWindow?.FinalizeStreamingRound();

            if (task.IsFaulted)
            {
                _chatWindow?.StopProgressDots();
                _chatWindow?.AddAiResponse(DeepJebLoc.ErrorPrefix + " " +
                    (task.Exception?.InnerException?.Message ?? DeepJebLoc.RequestFailed));
                // Persist the user message even on failure so /retry can recover it
                if (!string.IsNullOrEmpty(ChatSession.LastUserMessage))
                    ChatSession.Messages.Add(ChatMessage.CreateUser(ChatSession.LastUserMessage));
                AutoSaveSession();
                RefreshSessionList();
                _chatWindow?.RequestRedraw();
                yield break;
            }

            var pipelineResult = task.Result;
            if (pipelineResult != null && pipelineResult.Success)
            {
                // Thread safety: update Messages on main thread only
                ChatSession.Messages = pipelineResult.UpdatedConversation ?? ChatSession.Messages;
                ChatSession.TrimHistory();
                AutoSaveSession();
                RefreshSessionList();
            }
            else
            {
                string err = pipelineResult?.ErrorMessage ?? ChatSession.LastError ?? DeepJebLoc.UnknownError;
                _chatWindow?.AddAiResponse(DeepJebLoc.ErrorPrefix + " " + err);
                // Persist the user message even on pipeline failure
                if (!string.IsNullOrEmpty(ChatSession.LastUserMessage))
                    ChatSession.Messages.Add(ChatMessage.CreateUser(ChatSession.LastUserMessage));
                AutoSaveSession();
                RefreshSessionList();
            }

            _chatWindow?.RequestRedraw();
        }

        private void AutoSaveSession()
        {
            if (_sessionStore == null || ChatSession == null) return;
            try
            {
                // System prompt is injected at conversation start — never persist it.
                // Skip ALL System and Tool messages; keep only User and Assistant.
                var filtered = new List<ChatMessage>();
                foreach (var m in ChatSession.Messages)
                {
                    if (m.Role == ChatMessage.RoleType.System) continue;
                    if (m.Role == ChatMessage.RoleType.Tool) continue;
                    if (m.ToolCalls != null && m.ToolCalls.Count > 0) continue;
                    filtered.Add(m);
                }
                _sessionStore.Save(new SessionData
                {
                    Id = ChatSession.SessionId,
                    ProviderName = ChatSession.ProviderName,
                    ModelName = ChatSession.ModelName,
                    CreatedAt = ChatSession.CreatedAt,
                    Messages = filtered
                });
            }
            catch { /* Don't crash on save failure */ }
        }

        private void RefreshSessionList()
        {
            if (_chatWindow == null || _sessionStore == null) return;
            var sessions = _sessionStore.ListSessions();
            _chatWindow.SessionIds = sessions.ConvertAll(s => s.Id);
        }

        private void LoadSession(string sessionId)
        {
            if (_sessionStore == null || ChatSession == null) return;
            var data = _sessionStore.Load(sessionId);
            if (data == null) return;

            // Strip all old System messages — system prompt is injected fresh.
            var msgs = data.Messages ?? new List<ChatMessage>();
            msgs.RemoveAll(m => m.Role == ChatMessage.RoleType.System);
            // Prepend the current system prompt
            msgs.Insert(0, ChatMessage.CreateSystem(ChatPipeline?.SystemPrompt ?? ""));
            ChatSession.Messages = msgs;

            // Adopt the loaded session's ID so AutoSaveSession writes to the correct file
            ChatSession.SessionId = data.Id;
            ChatSession.CreatedAt = data.CreatedAt;
            ChatSession.LastUserMessage = null;

            // Switch to the loaded session's provider. If the provider no longer
            // exists, keep the current client to avoid client/model mismatch (400).
            bool switched = false;
            if (!string.IsNullOrEmpty(data.ProviderName))
                switched = SwitchToProviderByName(data.ProviderName);

            if (switched && !string.IsNullOrEmpty(data.ModelName))
            {
                var cfg = _providers.Find(p => p.Name == data.ProviderName);
                if (cfg != null && cfg.EnabledModels != null && cfg.EnabledModels.Contains(data.ModelName))
                    _activeModel = data.ModelName;
            }

            ChatSession.ProviderName = data.ProviderName;
            ChatSession.ModelName = data.ModelName;

            if (_chatWindow != null)
            {
                _chatWindow.ProviderName = data.ProviderName;
                _chatWindow.ModelName = data.ModelName;
                _chatWindow.RebuildDisplayFromMessages(ChatSession.Messages);
                SyncProviderLists();
            }
            Debug.Log("[DeepJeb] Session loaded: " + sessionId);
        }

        private void OnDestroy()
        {
            SaveConfig();
            if (Instance == this) Instance = null;
            Debug.Log("[DeepJeb] Shutdown.");
        }
    }
}
