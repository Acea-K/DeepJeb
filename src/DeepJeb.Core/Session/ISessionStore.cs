using System.Collections.Generic;
using DeepJeb.Core.Models;

namespace DeepJeb.Core.Session
{
    /// <summary>
    /// Persists and loads chat sessions. The store is responsible for
    /// serializing/deserializing session data to/from disk.
    /// </summary>
    public interface ISessionStore
    {
        /// <summary>Save a session to persistent storage.</summary>
        void Save(SessionData session);

        /// <summary>Load a session by its ID.</summary>
        SessionData Load(string sessionId);

        /// <summary>List all saved sessions, newest first.</summary>
        List<SessionMetadata> ListSessions();

        /// <summary>Delete a session from storage.</summary>
        void Delete(string sessionId);
    }

    /// <summary>
    /// Full session data including all messages.
    /// </summary>
    public class SessionData
    {
        public string Id { get; set; }
        public string ProviderName { get; set; }
        public string ModelName { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public List<ChatMessage> Messages { get; set; }
    }

    /// <summary>
    /// Lightweight session metadata for listing (no message bodies).
    /// </summary>
    public class SessionMetadata
    {
        public string Id { get; set; }
        public string ProviderName { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
