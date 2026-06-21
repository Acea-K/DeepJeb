using System.Threading.Tasks;

namespace DeepJeb.Core.Agent
{
    /// <summary>
    /// A tool the AI agent can invoke. Implementations are registered
    /// with the ToolRegistry and exposed to the model as function definitions.
    /// </summary>
    public interface ITool
    {
        /// <summary>Unique tool name exposed to the AI (e.g. "read_file").</summary>
        string Name { get; }

        /// <summary>Human-readable description for the AI to decide when to call.</summary>
        string Description { get; }

        /// <summary>
        /// JSON Schema describing the tool's parameters.
        /// Follows the OpenAI function-calling schema format.
        /// </summary>
        string ParametersSchema { get; }

        /// <summary>
        /// Execute the tool with the given JSON arguments.
        /// Returns the result as a string (typically JSON or plain text).
        /// </summary>
        Task<string> ExecuteAsync(string argumentsJson);
    }
}
