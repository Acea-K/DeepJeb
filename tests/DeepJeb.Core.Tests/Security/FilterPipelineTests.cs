using DeepJeb.Core.Security;
using NUnit.Framework;

namespace DeepJeb.Core.Tests.Security
{
    [TestFixture]
    public class FilterPipelineTests
    {
        private FilterPipeline _pipeline;

        [SetUp]
        public void SetUp()
        {
            _pipeline = new FilterPipeline();
        }

        [Test]
        public void Run_EmptyPipeline_ReturnsPass()
        {
            var result = _pipeline.Run("any content");
            Assert.That(result.Allowed, Is.True);
        }

        [Test]
        public void Run_AllFiltersPass_ReturnsPass()
        {
            _pipeline.AddFilter(new PassthroughFilter("A"));
            _pipeline.AddFilter(new PassthroughFilter("B"));

            var result = _pipeline.Run("hello");
            Assert.That(result.Allowed, Is.True);
        }

        [Test]
        public void Run_FirstFilterBlocks_ShortCircuits()
        {
            _pipeline.AddFilter(new BlockingFilter("blocker", "no reason"));
            _pipeline.AddFilter(new PassthroughFilter("never-reached"));

            var result = _pipeline.Run("content");
            Assert.That(result.Allowed, Is.False);
            Assert.That(result.BlockingFilter, Is.EqualTo("blocker"));
        }

        [Test]
        public void Run_SecondFilterBlocks_FirstPasses()
        {
            _pipeline.AddFilter(new PassthroughFilter("passes"));
            _pipeline.AddFilter(new BlockingFilter("second-blocker", "reason"));

            var result = _pipeline.Run("content");
            Assert.That(result.Allowed, Is.False);
            Assert.That(result.BlockingFilter, Is.EqualTo("second-blocker"));
        }

        [Test]
        public void Run_FiltersExecuteInOrder()
        {
            var order = new System.Collections.Generic.List<string>();
            _pipeline.AddFilter(new RecordingFilter("first", order, null));
            _pipeline.AddFilter(new RecordingFilter("second", order, null));
            _pipeline.AddFilter(new RecordingFilter("third", order, null));

            _pipeline.Run("test");

            Assert.That(order, Is.EqualTo(new[] { "first", "second", "third" }));
        }

        [Test]
        public void Run_ShortCircuit_StopsExecution()
        {
            var order = new System.Collections.Generic.List<string>();
            _pipeline.AddFilter(new RecordingFilter("first", order, null));
            _pipeline.AddFilter(new RecordingFilter("second", order, "block"));
            _pipeline.AddFilter(new RecordingFilter("third-never-reached", order, null));

            var result = _pipeline.Run("test");

            Assert.That(result.Allowed, Is.False);
            Assert.That(order, Is.EqualTo(new[] { "first", "second" }));
        }

        [Test]
        public void RemoveFilter_RemovesIt()
        {
            var blockingFilter = new BlockingFilter("removed", "nope");
            _pipeline.AddFilter(blockingFilter);
            _pipeline.RemoveFilter(blockingFilter);

            var result = _pipeline.Run("content");
            Assert.That(result.Allowed, Is.True);
        }

        // ---- Test doubles ----

        private class PassthroughFilter : IInterceptFilter
        {
            public string Name { get; }
            public PassthroughFilter(string name) { Name = name; }
            public string Inspect(string content) => null;
        }

        private class BlockingFilter : IInterceptFilter
        {
            private readonly string _reason;
            public string Name { get; }
            public BlockingFilter(string name, string reason) { Name = name; _reason = reason; }
            public string Inspect(string content) => _reason;
        }

        private class RecordingFilter : IInterceptFilter
        {
            private readonly System.Collections.Generic.List<string> _log;
            private readonly string _blockReason;
            public string Name { get; }
            public RecordingFilter(string name, System.Collections.Generic.List<string> log, string blockReason)
            {
                Name = name; _log = log; _blockReason = blockReason;
            }
            public string Inspect(string content)
            {
                _log.Add(Name);
                return _blockReason;
            }
        }
    }
}
