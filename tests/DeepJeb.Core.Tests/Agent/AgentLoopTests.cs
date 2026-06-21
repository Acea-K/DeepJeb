using System.Collections.Generic;
using System.Threading.Tasks;
using DeepJeb.Core.Agent;
using DeepJeb.Core.Models;
using NUnit.Framework;

namespace DeepJeb.Core.Tests.Agent
{
    [TestFixture]
    public class AgentLoopTests
    {
        private ToolRegistry _registry;
        private AgentLoop _loop;

        [SetUp]
        public void SetUp()
        {
            _registry = new ToolRegistry();
            _loop = new AgentLoop(_registry);
        }

        [Test]
        public async Task RunWithFunc_SimpleTextResponse_ReturnsImmediately()
        {
            var conversation = new List<ChatMessage>
            {
                ChatMessage.CreateSystem("You are helpful.")
            };

            var result = await _loop.RunWithFuncAsync(
                conversation,
                "Hi there",
                (msgs, tools) => Task.FromResult(new AiResponse
                {
                    Content = "Hello! How can I help?",
                    ToolCalls = null,
                    FinishReason = "stop"
                }));

            Assert.That(result.RoundsExecuted, Is.EqualTo(1));
            Assert.That(result.FinalResponse, Is.EqualTo("Hello! How can I help?"));
            Assert.That(result.WasTerminatedEarly, Is.False);
            Assert.That(result.UpdatedConversation.Count, Is.GreaterThan(conversation.Count));
        }

        [Test]
        public async Task RunWithFunc_AppendsUserMessage()
        {
            var conversation = new List<ChatMessage>
            {
                ChatMessage.CreateSystem("System")
            };

            var result = await _loop.RunWithFuncAsync(
                conversation,
                "My question",
                (msgs, tools) => Task.FromResult(new AiResponse
                {
                    Content = "Answer",
                    ToolCalls = null,
                    FinishReason = "stop"
                }));

            // Conversation should have: system, user, assistant
            Assert.That(result.UpdatedConversation.Count, Is.EqualTo(3));
            Assert.That(result.UpdatedConversation[1].Role, Is.EqualTo(ChatMessage.RoleType.User));
            Assert.That(result.UpdatedConversation[1].Content, Is.EqualTo("My question"));
            Assert.That(result.UpdatedConversation[2].Role, Is.EqualTo(ChatMessage.RoleType.Assistant));
        }

        [Test]
        public async Task RunWithFunc_ToolCall_ExecutesAndContinues()
        {
            // Register a fake tool
            var echoTool = new EchoTool();
            _registry.Register(echoTool);

            var conversation = new List<ChatMessage>
            {
                ChatMessage.CreateSystem("System")
            };

            int callCount = 0;

            var result = await _loop.RunWithFuncAsync(
                conversation,
                "Read a file",
                (msgs, tools) =>
                {
                    callCount++;
                    if (callCount == 1)
                    {
                        // First response: request tool call
                        return Task.FromResult(new AiResponse
                        {
                            Content = "Let me check.",
                            ToolCalls = new List<ToolCall>
                            {
                                new ToolCall { Id = "call_1", Name = "echo", Arguments = "{\"text\":\"hello\"}" }
                            },
                            FinishReason = "tool_calls"
                        });
                    }
                    else
                    {
                        // Second response: after tool result
                        return Task.FromResult(new AiResponse
                        {
                            Content = "The file contains hello.",
                            ToolCalls = null,
                            FinishReason = "stop"
                        });
                    }
                });

            Assert.That(result.RoundsExecuted, Is.EqualTo(2));
            Assert.That(result.FinalResponse, Is.EqualTo("The file contains hello."));
            Assert.That(result.WasTerminatedEarly, Is.False);

            // Conversation should contain the tool call and tool result messages
            Assert.That(result.UpdatedConversation.Count, Is.GreaterThanOrEqualTo(5));
        }

        [Test]
        public async Task RunWithFunc_MaxRoundsExceeded_Terminates()
        {
            var echoTool = new EchoTool();
            _registry.Register(echoTool);

            var config = new AgentLoopConfig { MaxRounds = 2, ForceSummaryRound = 999 };
            var conversation = new List<ChatMessage> { ChatMessage.CreateSystem("S") };

            var result = await _loop.RunWithFuncAsync(
                conversation,
                "test",
                (msgs, tools) =>
                {
                    // Always return tool calls to force the loop
                    return Task.FromResult(new AiResponse
                    {
                        Content = "calling tool...",
                        ToolCalls = new List<ToolCall>
                        {
                            new ToolCall { Id = "c", Name = "echo", Arguments = "{\"text\":\"x\"}" }
                        },
                        FinishReason = "tool_calls"
                    });
                },
                config);

            Assert.That(result.WasTerminatedEarly, Is.True);
            Assert.That(result.TerminationReason, Does.Contain("Max rounds"));
            Assert.That(result.RoundsExecuted, Is.EqualTo(2));
        }

        [Test]
        public async Task RunWithFunc_ForceSummary_ReturnsTextEarly()
        {
            var echoTool = new EchoTool();
            _registry.Register(echoTool);

            var config = new AgentLoopConfig { MaxRounds = 10, ForceSummaryRound = 1 };
            var conversation = new List<ChatMessage> { ChatMessage.CreateSystem("S") };

            var result = await _loop.RunWithFuncAsync(
                conversation,
                "test",
                (msgs, tools) =>
                {
                    return Task.FromResult(new AiResponse
                    {
                        Content = "Here's a summary of what I found.",
                        ToolCalls = new List<ToolCall>
                        {
                            new ToolCall { Id = "c", Name = "echo", Arguments = "{\"text\":\"x\"}" }
                        },
                        FinishReason = "tool_calls"
                    });
                },
                config);

            Assert.That(result.WasTerminatedEarly, Is.True);
            Assert.That(result.TerminationReason, Does.Contain("Force summary"));
            Assert.That(result.FinalResponse, Does.Contain("summary"));
        }

        [Test]
        public async Task RunWithFunc_ForceSummary_InjectsPromptAndMakesFinalCall()
        {
            var echoTool = new EchoTool();
            _registry.Register(echoTool);

            var config = new AgentLoopConfig { MaxRounds = 10, ForceSummaryRound = 2 };
            var conversation = new List<ChatMessage> { ChatMessage.CreateSystem("S") };
            int callCount = 0;
            List<ChatMessage> lastMessages = null;
            List<ITool> lastTools = null;

            var result = await _loop.RunWithFuncAsync(
                conversation, "test",
                (msgs, tools) =>
                {
                    callCount++;
                    lastMessages = msgs;
                    lastTools = tools;
                    // Always return tool calls without text (worst case for force summary)
                    return Task.FromResult(new AiResponse
                    {
                        Content = null,
                        ToolCalls = new List<ToolCall>
                        {
                            new ToolCall { Id = "c", Name = "echo", Arguments = "{\"text\":\"x\"}" }
                        },
                        FinishReason = "tool_calls"
                    });
                }, config);

            // Should have made 3 calls: round1, round2(force summary → final call without tools)
            Assert.That(callCount, Is.GreaterThanOrEqualTo(2));
            // Final call should have empty tools list
            Assert.That(lastTools, Is.Not.Null);
            Assert.That(lastTools.Count, Is.EqualTo(0));
            Assert.That(result.WasTerminatedEarly, Is.True);
            Assert.That(result.TerminationReason, Does.Contain("Force summary"));
        }

        [Test]
        public async Task RunWithFunc_WriteFile_InjectsConfirmationAndSummarizes()
        {
            var writeTool = new FakeWriteTool();
            _registry.Register(writeTool);

            var config = new AgentLoopConfig
            {
                MaxRounds = 10,
                ForceSummaryRound = 999,
                SummarizeAfterWrite = true
            };
            var conversation = new List<ChatMessage> { ChatMessage.CreateSystem("S") };
            int callCount = 0;

            var result = await _loop.RunWithFuncAsync(
                conversation, "write test.cfg",
                (msgs, tools) =>
                {
                    callCount++;
                    if (callCount == 1)
                    {
                        return Task.FromResult(new AiResponse
                        {
                            Content = "Writing file.",
                            ToolCalls = new List<ToolCall>
                            {
                                new ToolCall { Id = "w1", Name = "write_file",
                                    Arguments = "{\"path\":\"test.cfg\",\"content\":\"data\"}" }
                            },
                            FinishReason = "tool_calls"
                        });
                    }
                    // Call 2: write-file summary round
                    return Task.FromResult(new AiResponse
                    {
                        Content = "File written successfully. test.cfg now contains the configuration data.",
                        ToolCalls = null,
                        FinishReason = "stop"
                    });
                }, config);

            Assert.That(callCount, Is.EqualTo(2));
            Assert.That(result.FinalResponse, Does.Contain("written"));
            Assert.That(result.WasTerminatedEarly, Is.False);
        }

        [Test]
        public void RunAsync_WithoutSendFunction_Throws()
        {
            var conversation = new List<ChatMessage> { ChatMessage.CreateSystem("S") };

            var ex = Assert.ThrowsAsync<System.InvalidOperationException>(
                async () => await _loop.RunAsync(conversation, "test"));

            Assert.That(ex.Message, Does.Contain("SendFunction"));
        }

        // ---- Test double ----

        private class EchoTool : ITool
        {
            public string Name => "echo";
            public string Description => "Echoes back the input";
            public string ParametersSchema => @"{""type"":""object"",""properties"":{""text"":{""type"":""string""}}}";
            public Task<string> ExecuteAsync(string args) => Task.FromResult(args);
        }

        private class FakeWriteTool : ITool
        {
            public string Name => "write_file";
            public string Description => "Writes a file";
            public string ParametersSchema => @"{""type"":""object"",""properties"":{""path"":{""type"":""string""}}}";
            public Task<string> ExecuteAsync(string args) =>
                Task.FromResult("{\"success\":true,\"path\":\"test.cfg\",\"bytes_written\":42}");
        }
    }
}
