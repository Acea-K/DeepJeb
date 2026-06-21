using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DeepJeb.Core.Agent;
using DeepJeb.Core.Json;

namespace DeepJeb.Unity.Tools
{
    /// <summary>
    /// Reports the current KSP game state to the AI:
    /// active scene, current vessel, UT time, installed mods.
    /// Uses KSP's FlightGlobals, HighLogic, Planetarium APIs.
    /// </summary>
    public class GetGameStateTool : ITool
    {
        public string Name => "get_game_state";
        public string Description =>
            "Get the current KSP game state: active scene, current vessel name and situation, " +
            "Universal Time, celestial body, and list of installed mods in GameData.";

        public string ParametersSchema => @"{
            ""type"": ""object"",
            ""properties"": {},
            ""required"": []
        }";

        public Task<string> ExecuteAsync(string argumentsJson)
        {
            var state = new Dictionary<string, object>();

            // Current scene
            state["scene"] = HighLogic.LoadedScene.ToString();

            // Game time
            state["universal_time"] = Planetarium.GetUniversalTime();
            state["ut_string"] = KSPUtil.PrintTime(Planetarium.GetUniversalTime(), 3, true);

            // Active vessel
            if (FlightGlobals.ActiveVessel != null)
            {
                var v = FlightGlobals.ActiveVessel;
                state["active_vessel"] = v.vesselName;
                state["vessel_situation"] = v.situation.ToString();
                state["vessel_altitude"] = v.altitude;
                state["vessel_orbital_velocity"] = v.obt_velocity.magnitude;
                state["main_body"] = v.mainBody?.bodyName ?? "Unknown";
                state["vessel_mass"] = v.totalMass;

                // Crew info
                var crew = new List<string>();
                foreach (var p in v.GetVesselCrew())
                {
                    crew.Add(p.name + " (" + p.experienceTrait.Title + ")");
                }
                state["crew"] = crew;
            }
            else
            {
                state["active_vessel"] = null;
            }

            // Installed mods (directories under GameData, excluding Squad/SquadExpansion)
            var mods = new List<string>();
            string gameDataRoot = KSPUtil.ApplicationRootPath + "GameData";
            if (Directory.Exists(gameDataRoot))
            {
                foreach (var dir in Directory.GetDirectories(gameDataRoot))
                {
                    string dirName = Path.GetFileName(dir);
                    if (dirName != "Squad" && dirName != "SquadExpansion" && dirName != "DeepJeb")
                    {
                        mods.Add(dirName);
                    }
                }
            }
            state["installed_mods"] = mods;
            state["installed_mod_count"] = mods.Count;

            return Task.FromResult(JsonMapper.Stringify(state));
        }
    }
}
