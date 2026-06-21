using System.Collections.Generic;
using DeepJeb.Core.Context;
using DeepJeb.Core.Models;
using NUnit.Framework;

namespace DeepJeb.Core.Tests.Context
{
    [TestFixture]
    public class ContextManagerTests
    {
        private ContextManager _manager;

        [SetUp]
        public void SetUp()
        {
            _manager = new ContextManager();
        }

        // ---- Token Estimation ----

        [Test]
        public void EstimateTokenCount_EmptyList_ReturnsZero()
        {
            Assert.That(_manager.EstimateTokenCount(new List<ChatMessage>()), Is.EqualTo(0));
        }

        [Test]
        public void EstimateTokenCount_NullList_ReturnsZero()
        {
            Assert.That(_manager.EstimateTokenCount(null), Is.EqualTo(0));
        }

        [Test]
        public void EstimateTokenCount_SimpleMessage_ReturnsEstimate()
        {
            var messages = new List<ChatMessage>
            {
                ChatMessage.CreateUser("Hello, how are you?")
            };
            // "Hello, how are you?" = 19 chars → ceil(19/4) = 5 tokens
            int estimate = _manager.EstimateTokenCount(messages);
            Assert.That(estimate, Is.EqualTo(5));
        }

        [Test]
        public void EstimateTokenCount_MultipleMessages_SumsAll()
        {
            var messages = new List<ChatMessage>
            {
                ChatMessage.CreateSystem("You are helpful."),   // 16 chars → 4 tokens
                ChatMessage.CreateUser("Hi"),                    // 2 chars → 1 token
                ChatMessage.CreateAssistant("Hello!")            // 6 chars → 2 tokens
            };
            // Total: 16+2+6 = 24 chars → ceil(24/4) = 6 tokens
            Assert.That(_manager.EstimateTokenCount(messages), Is.EqualTo(6));
        }

        [Test]
        public void EstimateTokenCount_IncludesToolCallArgs()
        {
            var messages = new List<ChatMessage>
            {
                ChatMessage.CreateAssistant("Using tool", new List<ToolCall>
                {
                    new ToolCall { Id = "1", Name = "read", Arguments = "{\"path\":\"file.cfg\"}" }
                })
            };
            // "Using tool" = 10 chars + args 22 chars = 32 → 8 tokens
            int estimate = _manager.EstimateTokenCount(messages);
            Assert.That(estimate, Is.EqualTo(8));
        }

        // ---- Context Limits ----

        [Test]
        public void GetContextLimit_KnownModel_ReturnsLimit()
        {
            Assert.That(_manager.GetContextLimit("claude-sonnet-4-6"), Is.EqualTo(200000));
            Assert.That(_manager.GetContextLimit("gpt-4o"), Is.EqualTo(128000));
            Assert.That(_manager.GetContextLimit("deepseek-chat"), Is.EqualTo(1000000));
        }

        [Test]
        public void GetContextLimit_UnknownModel_ReturnsNull()
        {
            Assert.That(_manager.GetContextLimit("totally-unknown-model-v99"), Is.Null);
        }

        [Test]
        public void GetContextLimit_SubstringMatch_Works()
        {
            // "gpt-4o-2024-05-13" should match "gpt-4o"
            var limit = _manager.GetContextLimit("gpt-4o-2024-05-13");
            Assert.That(limit, Is.EqualTo(128000));
        }

        [Test]
        public void GetContextLimit_CaseInsensitive()
        {
            Assert.That(_manager.GetContextLimit("CLAUDE-SONNET-4-6"), Is.EqualTo(200000));
            Assert.That(_manager.GetContextLimit("Gpt-4o"), Is.EqualTo(128000));
        }

        [Test]
        public void SetContextLimit_StoresAndRetrieves()
        {
            _manager.SetContextLimit("my-custom-model", 65536);
            Assert.That(_manager.GetContextLimit("my-custom-model"), Is.EqualTo(65536));
        }

        [Test]
        public void HasKnownLimit_Works()
        {
            Assert.That(_manager.HasKnownLimit("gpt-4o"), Is.True);
            Assert.That(_manager.HasKnownLimit("unknown-model"), Is.False);
        }

        // ---- Truncation ----

        [Test]
        public void TruncateIfNeeded_UnderLimit_ReturnsOriginal()
        {
            var messages = new List<ChatMessage>
            {
                ChatMessage.CreateSystem("You are helpful."),
                ChatMessage.CreateUser("Hi"),
                ChatMessage.CreateAssistant("Hello")
            };

            var result = _manager.TruncateIfNeeded(messages, "gpt-4o");

            // 128000 * 0.9 = 115200 tokens, our ~7 tokens is well under
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public void TruncateIfNeeded_PreservesSystemPrompt()
        {
            // Create many messages to force truncation on a tiny limit
            _manager.SetContextLimit("tiny-model", 100); // 100 token limit → 90 token threshold

            var messages = new List<ChatMessage>
            {
                ChatMessage.CreateSystem("SYSTEM PROMPT HERE")
            };

            // Add enough messages to exceed the limit
            // Each message is ~14 chars → 4 tokens, so 30 messages ≈ 120 tokens
            for (int i = 0; i < 30; i++)
            {
                messages.Add(ChatMessage.CreateUser("Message number " + i));
                messages.Add(ChatMessage.CreateAssistant("Response number " + i));
            }

            var result = _manager.TruncateIfNeeded(messages, "tiny-model");

            // First message should still be the system prompt
            Assert.That(result[0].Role, Is.EqualTo(ChatMessage.RoleType.System));
            Assert.That(result[0].Content, Is.EqualTo("SYSTEM PROMPT HERE"));

            // Should have truncated (fewer messages than original)
            Assert.That(result.Count, Is.LessThan(messages.Count));
        }

        [Test]
        public void TruncateIfNeeded_KeepsRecentMessages()
        {
            _manager.SetContextLimit("tiny-model", 200);

            var messages = new List<ChatMessage>
            {
                ChatMessage.CreateSystem("System")
            };

            // Add sequentially numbered messages
            for (int i = 0; i < 20; i++)
            {
                messages.Add(ChatMessage.CreateUser("Q" + i));
                messages.Add(ChatMessage.CreateAssistant("A" + i));
            }

            var result = _manager.TruncateIfNeeded(messages, "tiny-model");

            // The last user message should be "Q19" (most recent)
            var lastUser = result.FindLast(m => m.Role == ChatMessage.RoleType.User);
            Assert.That(lastUser, Is.Not.Null);
            Assert.That(lastUser.Content, Is.EqualTo("Q19"));

            // The first user message should NOT be "Q0" (it got truncated)
            var allUserContent = result.ConvertAll(m => m.Content);
            Assert.That(allUserContent, Does.Not.Contain("Q0"));
        }

        [Test]
        public void TruncateIfNeeded_UnknownLimit_ReturnsOriginal()
        {
            var messages = new List<ChatMessage>
            {
                ChatMessage.CreateUser("test")
            };

            var result = _manager.TruncateIfNeeded(messages, "unknown-model");
            Assert.That(result.Count, Is.EqualTo(1)); // Unchanged
        }

        [Test]
        public void TruncateIfNeeded_SmallList_ReturnsOriginal()
        {
            var messages = new List<ChatMessage>
            {
                ChatMessage.CreateUser("hello")
            };

            var result = _manager.TruncateIfNeeded(messages, "gpt-4o");
            Assert.That(result.Count, Is.EqualTo(1));
        }
    }
}
