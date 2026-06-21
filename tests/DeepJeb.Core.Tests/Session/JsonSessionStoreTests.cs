using System;
using System.Collections.Generic;
using System.IO;
using DeepJeb.Core.Models;
using DeepJeb.Core.Session;
using NUnit.Framework;

namespace DeepJeb.Core.Tests.Session
{
    [TestFixture]
    public class JsonSessionStoreTests
    {
        private string _tempDir;
        private JsonSessionStore _store;

        [SetUp]
        public void SetUp()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "DeepJeb_StoreTest_" + Guid.NewGuid().ToString("N").Substring(0, 8));
            _store = new JsonSessionStore(_tempDir);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDir))
                Directory.Delete(_tempDir, true);
        }

        [Test]
        public void Save_CreatesFile()
        {
            var session = new SessionData
            {
                Id = "20260620-143021",
                ProviderName = "OpenAI",
                ModelName = "gpt-4o",
                Messages = new List<ChatMessage>
                {
                    ChatMessage.CreateSystem("You are helpful."),
                    ChatMessage.CreateUser("Hello")
                }
            };

            _store.Save(session);

            string filePath = Path.Combine(_tempDir, "20260620-143021.session");
            Assert.That(File.Exists(filePath), Is.True);
        }

        [Test]
        public void Save_ThenLoad_RoundTrip()
        {
            var original = new SessionData
            {
                Id = "20260620-143022",
                ProviderName = "Anthropic",
                ModelName = "claude-sonnet-4-6",
                Messages = new List<ChatMessage>
                {
                    ChatMessage.CreateSystem("You are a KSP expert."),
                    ChatMessage.CreateUser("How do I get to Duna?"),
                    ChatMessage.CreateAssistant("To get to Duna, you need about 1050 m/s...")
                }
            };

            _store.Save(original);
            var restored = _store.Load("20260620-143022");

            Assert.That(restored, Is.Not.Null);
            Assert.That(restored.Id, Is.EqualTo("20260620-143022"));
            Assert.That(restored.ProviderName, Is.EqualTo("Anthropic"));
            Assert.That(restored.ModelName, Is.EqualTo("claude-sonnet-4-6"));
            Assert.That(restored.Messages, Is.Not.Null);
            Assert.That(restored.Messages.Count, Is.EqualTo(3));
            Assert.That(restored.Messages[1].Content, Is.EqualTo("How do I get to Duna?"));
        }

        [Test]
        public void Load_NonExistent_ReturnsNull()
        {
            var result = _store.Load("nonexistent");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ListSessions_ReturnsSortedByDate()
        {
            var older = new SessionData
            {
                Id = "20260620-100000",
                ProviderName = "OpenAI",
                CreatedAt = new DateTime(2026, 6, 20, 10, 0, 0, DateTimeKind.Utc)
            };
            var newer = new SessionData
            {
                Id = "20260620-120000",
                ProviderName = "Anthropic",
                CreatedAt = new DateTime(2026, 6, 20, 12, 0, 0, DateTimeKind.Utc)
            };

            _store.Save(older);
            _store.Save(newer);

            var list = _store.ListSessions();

            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0].Id, Is.EqualTo("20260620-120000")); // Newest first
            Assert.That(list[1].Id, Is.EqualTo("20260620-100000"));
        }

        [Test]
        public void Delete_RemovesFile()
        {
            var session = new SessionData { Id = "todelete" };
            _store.Save(session);

            _store.Delete("todelete");

            Assert.That(_store.Load("todelete"), Is.Null);
        }

        [Test]
        public void Delete_NonExistent_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _store.Delete("nonexistent"));
        }

        [Test]
        public void Save_NullSession_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _store.Save(null));
        }

        [Test]
        public void Save_EmptyId_Throws()
        {
            Assert.Throws<ArgumentException>(() => _store.Save(new SessionData { Id = "" }));
        }

        [Test]
        public void GenerateSessionId_HasCorrectFormat()
        {
            var id = JsonSessionStore.GenerateSessionId();
            // yyyyMMdd-HHmmss
            Assert.That(id, Does.Match(@"^\d{8}-\d{6}$"));
        }
    }
}
