using System.Text;
using DeepJeb.Core.Json;

namespace DeepJeb.Core.Agent.Commands
{
    /// <summary>
    /// /game — Reports current KSP game state by invoking a callback
    /// set by the Mod layer. Core layer cannot reference KSP/Unity types,
    /// so game state is obtained via a delegate.
    /// </summary>
    public class GameCommand : ICommand
    {
        public string Name => "game";
        public string Description => "Show current KSP game state (scene, vessel, orbit, biomes, resources).";
        public string Usage => "/game";

        /// <summary>
        /// Delegate to fetch game state. Set by the Mod layer during initialization.
        /// Returns a JSON string with current game state, or null if unavailable.
        /// </summary>
        public System.Func<string> GameStateFetcher { get; set; }

        public CommandResult Execute(string args, CommandContext ctx)
        {
            if (GameStateFetcher == null)
                return new CommandResult { Success = false, Message = CommandMessages.GameStateUnavailable };

            string stateJson = GameStateFetcher();
            if (string.IsNullOrEmpty(stateJson))
                return new CommandResult { Success = false, Message = CommandMessages.NoGameState };

            // Parse JSON and format as readable text
            try
            {
                var state = MiniJson.Deserialize(stateJson) as System.Collections.Generic.Dictionary<string, object>;
                if (state == null)
                    return new CommandResult { Success = true, Message = stateJson };

                var sb = new StringBuilder();
                sb.AppendLine("=== KSP Game State ===");

                if (state.TryGetValue("scene", out var scene))
                    sb.AppendLine("Scene: " + scene);

                if (state.TryGetValue("active_vessel", out var vessel))
                    sb.AppendLine("Active Vessel: " + vessel);

                if (state.TryGetValue("orbit", out var orbit) && orbit != null)
                    sb.AppendLine("Orbit: " + orbit);

                if (state.TryGetValue("main_body", out var body))
                    sb.AppendLine("Main Body: " + body);

                if (state.TryGetValue("biome", out var biome) && biome != null)
                    sb.AppendLine("Biome: " + biome);

                if (state.TryGetValue("resources", out var resources) && resources != null)
                    sb.AppendLine("Resources: " + resources);

                if (state.TryGetValue("situation", out var situation))
                    sb.AppendLine("Situation: " + situation);

                if (state.TryGetValue("crew_count", out var crew))
                    sb.AppendLine("Crew: " + crew);

                // Show full raw state for any fields not explicitly formatted
                bool hasMore = false;
                foreach (var kvp in state)
                {
                    if (kvp.Key != "scene" && kvp.Key != "active_vessel" &&
                        kvp.Key != "orbit" && kvp.Key != "main_body" &&
                        kvp.Key != "biome" && kvp.Key != "resources" &&
                        kvp.Key != "situation" && kvp.Key != "crew_count")
                    {
                        if (!hasMore) { sb.AppendLine("---"); hasMore = true; }
                        sb.AppendLine(kvp.Key + ": " + kvp.Value);
                    }
                }

                return new CommandResult { Success = true, Message = sb.ToString().TrimEnd() };
            }
            catch (System.Exception ex)
            {
                return new CommandResult { Success = false, Message = string.Format(CommandMessages.GameStateParseFailed, ex.Message) };
            }
        }
    }
}
