using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeepJeb.Core.Models;

namespace DeepJeb.Core.Agent
{
    /// <summary>
    /// Thrown internally by ExecuteTools when repeat detection triggers.
    /// Caught by the main loop to return a clean AgentLoopResult.
    /// </summary>
    internal class AgentLoopTerminationException : Exception
    {
        public string FinalResponse { get; }
        public AgentLoopTerminationException(string reason, string finalResponse)
            : base(reason) { FinalResponse = finalResponse; }
    }

    /// <summary>
    /// Implements the agent tool-calling loop:
    ///
    /// 1. Send user message → receive AI response
    /// 2. If response contains tool calls, execute them via ToolRegistry
    /// 3. Send tool results back → continue loop
    /// 4. Stop when: AI returns text (no tools), max rounds reached,
    ///    or repeated results detected.
    ///
    /// The API call is delegated via a function parameter, keeping the loop
    /// independent of any specific Protocol implementation.
    /// </summary>
    public class AgentLoop : IAgentLoop
    {
        private readonly ToolRegistry _toolRegistry;
        private readonly AgentLoopConfig _defaultConfig;

        // Track the last N tool results for repeat detection
        private readonly Dictionary<string, Queue<string>> _toolResultHistory;

        /// <summary>Fires when a tool is about to be executed. (toolName, argumentsJson)</summary>
        public Action<string, string> OnToolExecuting { get; set; }

        public AgentLoop(ToolRegistry toolRegistry, AgentLoopConfig defaultConfig = null)
        {
            _toolRegistry = toolRegistry ?? throw new ArgumentNullException(nameof(toolRegistry));
            _defaultConfig = defaultConfig ?? new AgentLoopConfig();
            _toolResultHistory = new Dictionary<string, Queue<string>>();
        }

        /// <summary>
        /// Run the agent loop for one user message.
        /// The caller provides a sendFunc that handles the actual API communication.
        /// This keeps AgentLoop independent of any specific protocol.
        /// </summary>
        /// <param name="sendFunc">
        /// Function that sends messages to the AI and gets back a response.
        /// Signature: (messages, tools) → AiResponse
        /// </param>
        public async Task<AgentLoopResult> RunWithFuncAsync(
            List<ChatMessage> conversation,
            string userMessage,
            Func<List<ChatMessage>, List<ITool>, Task<AiResponse>> sendFunc,
            AgentLoopConfig config = null)
        {
            var cfg = config ?? _defaultConfig;
            var workingConversation = new List<ChatMessage>(conversation);

            // Append user message
            workingConversation.Add(ChatMessage.CreateUser(userMessage));

            int round = 0;
            _toolResultHistory.Clear();

            while (round < cfg.MaxRounds)
            {
                round++;

                // Get available tools
                var tools = new List<ITool>(_toolRegistry.GetAll());

                // Force text summary at round 5+
                bool forceSummary = round >= cfg.ForceSummaryRound;

                // Send to AI (delegated to caller)
                var response = await sendFunc(workingConversation, tools);

                // No tool calls → assistant returned text → done
                if (response.ToolCalls == null || response.ToolCalls.Count == 0)
                {
                    workingConversation.Add(ChatMessage.CreateAssistant(response.Content));
                    return new AgentLoopResult
                    {
                        FinalResponse = response.Content,
                        RoundsExecuted = round,
                        WasTerminatedEarly = false,
                        UpdatedConversation = workingConversation
                    };
                }

                // Force summary: if round limit hit + AI returned tool calls without text,
                // inject a summary prompt and make one final call without tools.
                if (forceSummary && string.IsNullOrEmpty(response.Content))
                {
                    workingConversation.Add(ChatMessage.CreateAssistant(null, response.ToolCalls));
                    try { await ExecuteTools(response.ToolCalls, workingConversation, cfg); }
                    catch (AgentLoopTerminationException ex)
                    {
                        return new AgentLoopResult { FinalResponse = ex.FinalResponse,
                            RoundsExecuted = round, WasTerminatedEarly = true,
                            TerminationReason = ex.Message, UpdatedConversation = workingConversation };
                    }

                    // Inject summary prompt + make final call without tools
                    workingConversation.Add(ChatMessage.CreateSystem(
                        "[PROCEDURAL] You have reached the maximum number of tool calls. " +
                        "Based on the information you have gathered, provide your final response. " +
                        "Do not request any more tools."));
                    var finalResponse = await sendFunc(workingConversation, new List<ITool>());
                    workingConversation.Add(ChatMessage.CreateAssistant(finalResponse.Content ?? ""));

                    return new AgentLoopResult
                    {
                        FinalResponse = finalResponse.Content ?? "",
                        RoundsExecuted = round,
                        WasTerminatedEarly = true,
                        TerminationReason = "Force summary at round " + round,
                        UpdatedConversation = workingConversation
                    };
                }

                // Force summary: AI returned text alongside tool calls — use text directly.
                // Strip tool calls from conversation since they won't be executed.
                if (forceSummary && !string.IsNullOrEmpty(response.Content))
                {
                    workingConversation.Add(ChatMessage.CreateAssistant(response.Content));
                    return new AgentLoopResult
                    {
                        FinalResponse = response.Content,
                        RoundsExecuted = round,
                        WasTerminatedEarly = true,
                        TerminationReason = "Force summary at round " + round,
                        UpdatedConversation = workingConversation
                    };
                }

                // Normal tool execution
                workingConversation.Add(ChatMessage.CreateAssistant(response.Content, response.ToolCalls));
                bool anyWriteFile;
                try { anyWriteFile = await ExecuteTools(response.ToolCalls, workingConversation, cfg); }
                catch (AgentLoopTerminationException ex)
                {
                    return new AgentLoopResult { FinalResponse = ex.FinalResponse,
                        RoundsExecuted = round, WasTerminatedEarly = true,
                        TerminationReason = ex.Message, UpdatedConversation = workingConversation };
                }

                // Write-file summary: inject confirmation prompt + make one extra call
                if (anyWriteFile && cfg.SummarizeAfterWrite && round < cfg.MaxRounds)
                {
                    workingConversation.Add(ChatMessage.CreateSystem(
                        "[PROCEDURAL] The file has been written successfully. " +
                        "Please confirm the operation and summarize what was written."));
                    var summaryResponse = await sendFunc(workingConversation, tools);

                    if (summaryResponse.ToolCalls == null || summaryResponse.ToolCalls.Count == 0)
                    {
                        workingConversation.Add(ChatMessage.CreateAssistant(summaryResponse.Content));
                        return new AgentLoopResult
                        {
                            FinalResponse = summaryResponse.Content,
                            RoundsExecuted = round,
                            WasTerminatedEarly = false,
                            UpdatedConversation = workingConversation
                        };
                    }
                    // AI wants more tools after write — let it continue
                    workingConversation.Add(ChatMessage.CreateAssistant(
                        summaryResponse.Content, summaryResponse.ToolCalls));
                    try { await ExecuteTools(summaryResponse.ToolCalls, workingConversation, cfg); }
                    catch (AgentLoopTerminationException ex)
                    {
                        return new AgentLoopResult { FinalResponse = ex.FinalResponse,
                            RoundsExecuted = round, WasTerminatedEarly = true,
                            TerminationReason = ex.Message, UpdatedConversation = workingConversation };
                    }
                    continue;
                }
            }

            // Max rounds reached
            return new AgentLoopResult
            {
                FinalResponse = "Maximum tool-call rounds reached.",
                RoundsExecuted = round,
                WasTerminatedEarly = true,
                TerminationReason = "Max rounds (" + cfg.MaxRounds + ") reached",
                UpdatedConversation = workingConversation
            };
        }

        /// <summary>
        /// Standard RunAsync using an injected send function (set via property for Phase 1).
        /// In Phase 2 this will be wired to the Protocol layer.
        /// </summary>
        public Func<List<ChatMessage>, List<ITool>, Task<AiResponse>> SendFunction { get; set; }

        public async Task<AgentLoopResult> RunAsync(
            List<ChatMessage> conversation,
            string userMessage,
            AgentLoopConfig config = null)
        {
            if (SendFunction == null)
                throw new InvalidOperationException(
                    "SendFunction must be set before calling RunAsync. " +
                    "Wire it to a protocol client in Phase 2.");

            return await RunWithFuncAsync(conversation, userMessage, SendFunction, config);
        }

        /// <summary>
        /// Execute a batch of tool calls and append results to the conversation.
        /// Returns true if any tool was a write_file.
        /// Throws AgentLoopTerminationException if repeat detection triggers.
        /// </summary>
        private async Task<bool> ExecuteTools(
            List<ToolCall> toolCalls,
            List<ChatMessage> conversation,
            AgentLoopConfig config)
        {
            bool anyWriteFile = false;

            foreach (var tc in toolCalls)
            {
                var tool = _toolRegistry.Get(tc.Name);
                string result;

                if (tool != null)
                {
                    OnToolExecuting?.Invoke(tc.Name, tc.Arguments);
                    result = await tool.ExecuteAsync(tc.Arguments);

                    if (tc.Name == "write_file")
                        anyWriteFile = true;

                    string repeatKey = tc.Name + ":" + NormalizeJson(tc.Arguments);
                    if (DetectRepeat(repeatKey, result, config.MaxRepeatedResults))
                    {
                        conversation.Add(ChatMessage.CreateTool(tc.Id, tc.Name, result));
                        throw new AgentLoopTerminationException(
                            $"Tool '{tc.Name}' returned same result {config.MaxRepeatedResults} times",
                            result);
                    }
                }
                else
                {
                    result = $"{{\"error\": \"Unknown tool: {tc.Name}\"}}";
                }

                conversation.Add(ChatMessage.CreateTool(tc.Id, tc.Name, result));
            }

            return anyWriteFile;
        }

        /// <summary>
        /// Track tool results and detect when the same tool+args combination
        /// returns the same result N times consecutively.
        /// </summary>
        private static string NormalizeJson(string json)
        {
            if (string.IsNullOrEmpty(json)) return json;
            var obj = DeepJeb.Core.Json.MiniJson.Deserialize(json) as Dictionary<string, object>;
            if (obj == null) return json;
            return DeepJeb.Core.Json.MiniJson.Serialize(new SortedDictionary<string, object>(obj));
        }

        private bool DetectRepeat(string key, string result, int maxRepeats)
        {
            if (!_toolResultHistory.TryGetValue(key, out var queue))
            {
                queue = new Queue<string>();
                _toolResultHistory[key] = queue;
            }

            // Check if the last result is the same
            if (queue.Count > 0)
            {
                var last = queue.Peek(); // Front of queue; all entries are identical so peek equals any result
                if (last == result)
                {
                    queue.Enqueue(result);
                    if (queue.Count >= maxRepeats)
                        return true;
                }
                else
                {
                    // Different result — reset
                    queue.Clear();
                    queue.Enqueue(result);
                }
            }
            else
            {
                queue.Enqueue(result);
            }

            return false;
        }
    }
}
