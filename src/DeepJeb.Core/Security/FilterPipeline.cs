using System.Collections.Generic;

namespace DeepJeb.Core.Security
{
    /// <summary>
    /// Ordered pipeline of intercept filters.
    /// Content passes through each filter in registration order.
    /// First filter to block short-circuits the pipeline.
    /// </summary>
    public class FilterPipeline
    {
        private readonly List<IInterceptFilter> _filters = new List<IInterceptFilter>();

        public void AddFilter(IInterceptFilter filter)
        {
            _filters.Add(filter);
        }

        public void RemoveFilter(IInterceptFilter filter)
        {
            _filters.Remove(filter);
        }

        /// <summary>
        /// Reset all stateful filters. Calls Reset() on every registered filter.
        /// Call when starting a new conversation.
        /// </summary>
        public void Reset()
        {
            foreach (var filter in _filters)
                filter.Reset();
        }

        /// <summary>
        /// Run content through all filters. Returns the first block, or Pass if all allow.
        /// </summary>
        public FilterResult Run(string content)
        {
            foreach (var filter in _filters)
            {
                var reason = filter.Inspect(content);
                if (reason != null)
                {
                    return FilterResult.Block(filter.Name, reason);
                }
            }
            return FilterResult.Pass();
        }
    }
}
