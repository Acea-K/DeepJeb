using System.Collections.Generic;

namespace DeepJeb.Core.Agent
{
    /// <summary>
    /// Registry of all tools available to the AI agent.
    /// Tools are registered at startup; the agent loop queries
    /// this registry to build function definitions and dispatch calls.
    /// </summary>
    public class ToolRegistry
    {
        private readonly Dictionary<string, ITool> _tools = new Dictionary<string, ITool>();

        public void Register(ITool tool)
        {
            _tools[tool.Name] = tool;
        }

        public void Unregister(string name)
        {
            _tools.Remove(name);
        }

        public ITool Get(string name)
        {
            _tools.TryGetValue(name, out var tool);
            return tool;
        }

        public IEnumerable<ITool> GetAll()
        {
            return _tools.Values;
        }

        public bool HasTool(string name)
        {
            return _tools.ContainsKey(name);
        }
    }
}
