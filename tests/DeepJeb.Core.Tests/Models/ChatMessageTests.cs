using System.Collections.Generic;
using DeepJeb.Core.Json;
using DeepJeb.Core.Models;
using NUnit.Framework;

namespace DeepJeb.Core.Tests.Models
{
    [TestFixture]
    public class ChatMessageTests
    {
        [Test]
        public void Constructor_GeneratesNonEmptyId()
        {
            var msg = new ChatMessage();
            Assert.That(msg.Id, Is.Not.Null.And.Not.Empty);
            Assert.That(msg.Id.Length, Is.EqualTo(32)); // Guid N format = 32 hex chars
        }

        [Test]
        public void Constructor_GeneratesUniqueIds()
        {
            var a = new ChatMessage();
            var b = new ChatMessage();
            Assert.That(a.Id, Is.Not.EqualTo(b.Id));
        }

        [Test]
        public void Constructor_SetsTimestampToUtcNow()
        {
            var before = DateTime.UtcNow.AddSeconds(-1);
            var msg = new ChatMessage();
            var after = DateTime.UtcNow.AddSeconds(1);
            Assert.That(msg.Timestamp, Is.GreaterThanOrEqualTo(before));
            Assert.That(msg.Timestamp, Is.LessThanOrEqualTo(after));
        }

        [Test]
        public void CreateSystem_SetsCorrectRoleAndContent()
        {
            var msg = ChatMessage.CreateSystem("You are helpful.");
            Assert.That(msg.Role, Is.EqualTo(ChatMessage.RoleType.System));
            Assert.That(msg.Content, Is.EqualTo("You are helpful."));
            Assert.That(msg.Id, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void CreateUser_SetsCorrectRoleAndContent()
        {
            var msg = ChatMessage.CreateUser("Hello!");
            Assert.That(msg.Role, Is.EqualTo(ChatMessage.RoleType.User));
            Assert.That(msg.Content, Is.EqualTo("Hello!"));
        }

        [Test]
        public void CreateAssistant_SetsCorrectRoleAndContent()
        {
            var msg = ChatMessage.CreateAssistant("Hi there.");
            Assert.That(msg.Role, Is.EqualTo(ChatMessage.RoleType.Assistant));
            Assert.That(msg.Content, Is.EqualTo("Hi there."));
        }

        [Test]
        public void CreateAssistant_WithToolCalls_StoresThem()
        {
            var toolCalls = new List<ToolCall>
            {
                new ToolCall { Id = "call_1", Name = "read_file", Arguments = "{\"path\":\"foo.cfg\"}" }
            };
            var msg = ChatMessage.CreateAssistant("Let me check.", toolCalls);
            Assert.That(msg.Role, Is.EqualTo(ChatMessage.RoleType.Assistant));
            Assert.That(msg.ToolCalls, Is.Not.Null);
            Assert.That(msg.ToolCalls.Count, Is.EqualTo(1));
            Assert.That(msg.ToolCalls[0].Name, Is.EqualTo("read_file"));
        }

        [Test]
        public void CreateTool_SetsAllFields()
        {
            var msg = ChatMessage.CreateTool("call_abc", "read_file", "file contents here");
            Assert.That(msg.Role, Is.EqualTo(ChatMessage.RoleType.Tool));
            Assert.That(msg.ToolCallId, Is.EqualTo("call_abc"));
            Assert.That(msg.ToolName, Is.EqualTo("read_file"));
            Assert.That(msg.Content, Is.EqualTo("file contents here"));
        }

        [Test]
        public void Serialize_RoundTrip_PreservesAllFields()
        {
            var original = new ChatMessage
            {
                Role = ChatMessage.RoleType.Assistant,
                Content = "Hello world",
                ToolCalls = new List<ToolCall>
                {
                    new ToolCall { Id = "tc1", Name = "test_tool", Arguments = "{\"x\":1}" }
                }
            };

            var json = JsonMapper.ToJson(original);
            var restored = JsonMapper.FromJson<ChatMessage>(json);

            Assert.That(restored.Id, Is.EqualTo(original.Id));
            Assert.That(restored.Role, Is.EqualTo(ChatMessage.RoleType.Assistant));
            Assert.That(restored.Content, Is.EqualTo("Hello world"));
            Assert.That(restored.ToolCalls, Is.Not.Null);
            Assert.That(restored.ToolCalls.Count, Is.EqualTo(1));
            Assert.That(restored.ToolCalls[0].Name, Is.EqualTo("test_tool"));
            Assert.That(restored.ToolCalls[0].Arguments, Is.EqualTo("{\"x\":1}"));
        }

        [Test]
        public void Serialize_RoundTrip_HandlesNullToolCalls()
        {
            var original = ChatMessage.CreateUser("simple message");
            var json = JsonMapper.ToJson(original);
            var restored = JsonMapper.FromJson<ChatMessage>(json);

            Assert.That(restored.Role, Is.EqualTo(ChatMessage.RoleType.User));
            Assert.That(restored.Content, Is.EqualTo("simple message"));
            Assert.That(restored.ToolCalls, Is.Null);
        }
    }

    [TestFixture]
    public class ToolCallTests
    {
        [Test]
        public void Serialize_RoundTrip_PreservesFields()
        {
            var original = new ToolCall
            {
                Id = "call_123",
                Name = "write_file",
                Arguments = "{\"path\":\"test.cfg\",\"content\":\"data\"}"
            };

            var json = JsonMapper.ToJson(original);
            var restored = JsonMapper.FromJson<ToolCall>(json);

            Assert.That(restored.Id, Is.EqualTo("call_123"));
            Assert.That(restored.Name, Is.EqualTo("write_file"));
            Assert.That(restored.Arguments, Is.EqualTo("{\"path\":\"test.cfg\",\"content\":\"data\"}"));
        }
    }
}
