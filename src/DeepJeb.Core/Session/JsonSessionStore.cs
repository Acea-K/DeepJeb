using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeepJeb.Core.Json;
using DeepJeb.Core.Models;

namespace DeepJeb.Core.Session
{
    /// <summary>
    /// JSON file-based session store.
    /// Sessions are saved as individual .session files in a directory.
    /// Session IDs use the format yyyyMMdd-HHmmss.
    /// Uses DeepJeb's built-in JsonMapper (zero external dependencies).
    /// </summary>
    public class JsonSessionStore : ISessionStore
    {
        private readonly string _sessionsDirectory;

        public JsonSessionStore(string sessionsDirectory)
        {
            _sessionsDirectory = sessionsDirectory;
            if (!Directory.Exists(_sessionsDirectory))
                Directory.CreateDirectory(_sessionsDirectory);
        }

        public void Save(SessionData session)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (string.IsNullOrEmpty(session.Id))
                throw new ArgumentException("Session must have a non-empty Id.");

            if (session.CreatedAt == default)
                session.CreatedAt = DateTime.UtcNow;

            string filePath = GetSessionPath(session.Id);
            string json = JsonMapper.ToJson(session);
            File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);
        }

        public SessionData Load(string sessionId)
        {
            string filePath = GetSessionPath(sessionId);
            if (!File.Exists(filePath))
                return null;

            string json = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            return JsonMapper.FromJson<SessionData>(json);
        }

        public List<SessionMetadata> ListSessions()
        {
            var result = new List<SessionMetadata>();

            if (!Directory.Exists(_sessionsDirectory))
                return result;

            var files = Directory.GetFiles(_sessionsDirectory, "*.session");
            foreach (var file in files)
            {
                try
                {
                    string json = File.ReadAllText(file, System.Text.Encoding.UTF8);
                    var session = JsonMapper.FromJson<SessionData>(json);
                    if (session != null)
                    {
                        result.Add(new SessionMetadata
                        {
                            Id = session.Id,
                            ProviderName = session.ProviderName,
                            CreatedAt = session.CreatedAt
                        });
                    }
                }
                catch
                {
                    // Skip corrupted files
                }
            }

            return result.OrderByDescending(s => s.CreatedAt).ToList();
        }

        public void Delete(string sessionId)
        {
            string filePath = GetSessionPath(sessionId);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        private string GetSessionPath(string sessionId)
        {
            string safeId = Path.GetFileName(sessionId);
            return Path.Combine(_sessionsDirectory, safeId + ".session");
        }

        public static string GenerateSessionId()
        {
            return DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");
        }
    }
}
