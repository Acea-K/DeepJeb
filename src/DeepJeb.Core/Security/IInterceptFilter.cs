namespace DeepJeb.Core.Security
{
    /// <summary>
    /// A pluggable filter in the security pipeline.
    /// Each filter inspects content and can block it with a reason.
    /// </summary>
    public interface IInterceptFilter
    {
        /// <summary>Human-readable name for logging.</summary>
        string Name { get; }

        /// <summary>
        /// Inspect the given content. Returns null if allowed;
        /// returns a block reason string if the content should be rejected.
        /// </summary>
        string Inspect(string content);

        /// <summary>
        /// Reset any stateful tracking (e.g., soft keyword counters).
        /// Call between sessions.
        /// </summary>
        void Reset();
    }

    /// <summary>
    /// Result of running content through the full filter pipeline.
    /// </summary>
    public class FilterResult
    {
        public bool Allowed { get; set; }
        public string BlockReason { get; set; }
        public string BlockingFilter { get; set; }

        public static FilterResult Pass()
        {
            return new FilterResult { Allowed = true };
        }

        public static FilterResult Block(string filterName, string reason)
        {
            return new FilterResult
            {
                Allowed = false,
                BlockingFilter = filterName,
                BlockReason = reason
            };
        }
    }
}
