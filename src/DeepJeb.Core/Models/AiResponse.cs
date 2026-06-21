using System.Collections.Generic;

namespace DeepJeb.Core.Models
{
    /// <summary>
    /// Raw response from an AI model before agent-loop processing.
    /// Avoids System.ValueTuple which isn't available in KSP's Mono runtime.
    /// </summary>
    public class AiResponse
    {
        public string Content { get; set; }
        public List<ToolCall> ToolCalls { get; set; }
        public string FinishReason { get; set; }
    }
}
