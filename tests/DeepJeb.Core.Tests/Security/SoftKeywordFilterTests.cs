using DeepJeb.Core.Security;
using NUnit.Framework;

namespace DeepJeb.Core.Tests.Security
{
    [TestFixture]
    public class SoftKeywordFilterTests
    {
        private SoftKeywordFilter _filter;

        [SetUp]
        public void SetUp()
        {
            _filter = new SoftKeywordFilter();
        }

        [Test]
        public void Inspect_CleanContent_ReturnsNull()
        {
            Assert.That(_filter.Inspect("How do I optimize my rocket design?"), Is.Null);
            Assert.That(_filter.Inspect("What's the best way to get to Duna?"), Is.Null);
        }

        [Test]
        public void Inspect_EmptyString_ReturnsNull()
        {
            Assert.That(_filter.Inspect(""), Is.Null);
        }

        [Test]
        public void Inspect_NullContent_ReturnsNull()
        {
            Assert.That(_filter.Inspect(null), Is.Null);
        }

        [Test]
        public void Inspect_FirstSoftKeywordHit_DoesNotBlock()
        {
            // First hit should pass (threshold is 2)
            var result = _filter.Inspect("that mod is stupid and doesn't work");
            Assert.That(result, Is.Null);
            Assert.That(_filter.CurrentHits, Is.EqualTo(1));
        }

        [Test]
        public void Inspect_SecondDifferentSoftKeyword_Blocks()
        {
            // Hit 1: "stupid"
            _filter.Inspect("that mod is stupid");
            Assert.That(_filter.CurrentHits, Is.EqualTo(1));

            // Hit 2: "kill" — should block
            var result = _filter.Inspect("I want to kill that bug");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.Contain("threshold reached"));
            Assert.That(_filter.CurrentHits, Is.EqualTo(2));
        }

        [Test]
        public void Inspect_SameKeywordTwice_CountsOnce()
        {
            // Same keyword multiple times should only count once per session
            _filter.Inspect("this is stupid");
            _filter.Inspect("this is also stupid");
            _filter.Inspect("this is stupid again");

            Assert.That(_filter.CurrentHits, Is.EqualTo(1));
        }

        [Test]
        public void Inspect_ThreeHits_StaysAtThree()
        {
            _filter.Inspect("stupid");
            _filter.Inspect("kill");
            _filter.Inspect("idiot");

            Assert.That(_filter.CurrentHits, Is.EqualTo(3));
        }

        [Test]
        public void Reset_ClearsState()
        {
            _filter.Inspect("stupid");
            _filter.Inspect("kill");

            Assert.That(_filter.CurrentHits, Is.EqualTo(2));

            _filter.Reset();

            Assert.That(_filter.CurrentHits, Is.EqualTo(0));

            // After reset, first hit should not block again
            var result = _filter.Inspect("that is stupid");
            Assert.That(result, Is.Null);
            Assert.That(_filter.CurrentHits, Is.EqualTo(1));
        }

        [Test]
        public void Inspect_MultipleMatchesInOneMessage_CountsAll()
        {
            // "stupid" and "kill" in one message = 2 unique hits = block
            var result = _filter.Inspect("this stupid program should kill itself");
            Assert.That(result, Is.Not.Null);
            Assert.That(_filter.CurrentHits, Is.EqualTo(2));
        }

        [Test]
        public void Inspect_CustomThreshold_Respected()
        {
            var highThresholdFilter = new SoftKeywordFilter(null, 5);

            highThresholdFilter.Inspect("stupid");
            highThresholdFilter.Inspect("kill");
            highThresholdFilter.Inspect("idiot");
            highThresholdFilter.Inspect("shoot");

            // 4 hits < threshold 5, should still pass
            var result = highThresholdFilter.Inspect("dumb");
            Assert.That(result, Is.Null);
            Assert.That(highThresholdFilter.CurrentHits, Is.EqualTo(5));

            // 6th hit would block... actually no, 5th hit already brought it to threshold
            // Let me re-test: 4 hits, then 5th should push to threshold 5
            // Actually: after 4 hits (3+2+1), then "dumb" is the 5th hit
            // So after this, CurrentHits=5, threshold=5, next inspect blocks
            var blockResult = highThresholdFilter.Inspect("weapon");
            Assert.That(blockResult, Is.Not.Null);
        }

        [Test]
        public void Inspect_CaseInsensitive_Matches()
        {
            _filter.Inspect("StUpId");
            Assert.That(_filter.CurrentHits, Is.EqualTo(1));

            _filter.Inspect("KILL");
            Assert.That(_filter.CurrentHits, Is.EqualTo(2));
        }

        [Test]
        public void Name_ReturnsCorrectFilterName()
        {
            Assert.That(_filter.Name, Is.EqualTo("SoftKeywordFilter"));
        }
    }
}
