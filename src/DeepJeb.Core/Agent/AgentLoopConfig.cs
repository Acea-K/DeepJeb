namespace DeepJeb.Core.Agent
{
    /// <summary>
    /// Configuration for the agent tool-calling loop.
    /// </summary>
    public class AgentLoopConfig
    {
        /// <summary>Maximum tool-call rounds per user message.</summary>
        public int MaxRounds { get; set; } = 10;

        /// <summary>Force a text summary starting at this round.</summary>
        public int ForceSummaryRound { get; set; } = 5;

        /// <summary>
        /// If the same tool returns the same result this many times
        /// consecutively, terminate the loop.
        /// </summary>
        public int MaxRepeatedResults { get; set; } = 6;

        /// <summary>Generate a natural-language summary after a successful write_file.</summary>
        public bool SummarizeAfterWrite { get; set; } = true;
    }
}
