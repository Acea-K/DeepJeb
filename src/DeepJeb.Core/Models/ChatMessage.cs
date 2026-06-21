using System;
using System.Collections.Generic;

namespace DeepJeb.Core.Models
{
    /// <summary>
    /// A single message in a chat conversation.
    /// Platform-agnostic; used by all layers.
    ///
    /// JSON field names follow the OpenAI convention (lowercase, snake_case for
    /// compound names). Serialization is handled by DeepJeb.Core.Json.JsonMapper.
    /// </summary>
    public class ChatMessage
    {
        public enum RoleType
        {
            System,
            User,
            Assistant,
            Tool
        }

        /// <summary>Unique message identifier (32-char hex GUID).</summary>
        public string Id { get; set; }

        /// <summary>Message role: system, user, assistant, or tool.</summary>
        public RoleType Role { get; set; }

        /// <summary>Message content text.</summary>
        public string Content { get; set; }

        /// <summary>Tool call ID (for tool result messages).</summary>
        public string ToolCallId { get; set; }

        /// <summary>Tool name (for tool result messages).</summary>
        public string ToolName { get; set; }

        /// <summary>Tool calls requested by the assistant (null if none).</summary>
        public List<ToolCall> ToolCalls { get; set; }

        /// <summary>UTC timestamp when the message was created.</summary>
        public DateTime Timestamp { get; set; }

        public ChatMessage()
        {
            Id = Guid.NewGuid().ToString("N");
            Timestamp = DateTime.UtcNow;
        }

        // ---- Factory methods ----

        public static ChatMessage CreateSystem(string content)
        {
            return new ChatMessage { Role = RoleType.System, Content = content };
        }

        public static ChatMessage CreateUser(string content)
        {
            return new ChatMessage { Role = RoleType.User, Content = content };
        }

        public static ChatMessage CreateAssistant(string content, List<ToolCall> toolCalls = null)
        {
            return new ChatMessage { Role = RoleType.Assistant, Content = content, ToolCalls = toolCalls };
        }

        public static ChatMessage CreateTool(string toolCallId, string toolName, string content)
        {
            return new ChatMessage
            {
                Role = RoleType.Tool,
                ToolCallId = toolCallId,
                ToolName = toolName,
                Content = content
            };
        }
    }

    /// <summary>
    /// A tool call requested by the AI model.
    /// </summary>
    public class ToolCall
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Arguments { get; set; } // JSON string
    }
}
