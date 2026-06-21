using System;
using System.IO;
using DeepJeb.Core.Security;
using NUnit.Framework;

namespace DeepJeb.Core.Tests.Security
{
    [TestFixture]
    public class PathSandboxTests
    {
        private string _tempRoot;
        private PathSandbox _sandbox;

        [SetUp]
        public void SetUp()
        {
            _tempRoot = Path.Combine(Path.GetTempPath(), "DeepJeb_SandboxTest_" + Guid.NewGuid().ToString("N").Substring(0, 8));
            Directory.CreateDirectory(_tempRoot);
            Directory.CreateDirectory(Path.Combine(_tempRoot, "Squad"));
            Directory.CreateDirectory(Path.Combine(_tempRoot, "SquadExpansion"));
            Directory.CreateDirectory(Path.Combine(_tempRoot, "MyMod"));
            _sandbox = new PathSandbox(_tempRoot);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempRoot))
                Directory.Delete(_tempRoot, true);
        }

        [Test]
        public void Resolve_ValidRelativePath_ReturnsFullPath()
        {
            var result = _sandbox.Resolve("MyMod/part.cfg");
            Assert.That(result, Is.EqualTo(Path.GetFullPath(Path.Combine(_tempRoot, "MyMod/part.cfg"))));
        }

        [Test]
        public void Resolve_SimpleFileName_Works()
        {
            var result = _sandbox.Resolve("readme.txt");
            Assert.That(result, Is.EqualTo(Path.GetFullPath(Path.Combine(_tempRoot, "readme.txt"))));
        }

        [Test]
        public void Resolve_NestedDirectory_Works()
        {
            var result = _sandbox.Resolve("MyMod/SubDir/deep/nested.cfg");
            Assert.That(result, Is.EqualTo(Path.GetFullPath(Path.Combine(_tempRoot, "MyMod/SubDir/deep/nested.cfg"))));
        }

        [Test]
        public void Resolve_EmptyPath_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sandbox.Resolve(""));
        }

        [Test]
        public void Resolve_NullPath_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sandbox.Resolve(null));
        }

        [Test]
        public void Resolve_PathWithDotDot_Throws()
        {
            var ex = Assert.Throws<UnauthorizedAccessException>(() => _sandbox.Resolve("../escape.txt"));
            Assert.That(ex.Message, Does.Contain("traversal"));
        }

        [Test]
        public void Resolve_PathWithDoubleDotMidPath_Throws()
        {
            var ex = Assert.Throws<UnauthorizedAccessException>(() => _sandbox.Resolve("MyMod/../../Squad/part.cfg"));
            Assert.That(ex.Message, Does.Contain("traversal"));
        }

        [Test]
        public void Resolve_PathWithTilde_Throws()
        {
            var ex = Assert.Throws<UnauthorizedAccessException>(() => _sandbox.Resolve("~/somefile.cfg"));
            Assert.That(ex.Message, Does.Contain("Home directory"));
        }

        [Test]
        public void Resolve_AbsolutePath_Throws()
        {
            // Use an absolute path
            var absPath = Path.Combine(_tempRoot, "MyMod/file.cfg");
            var ex = Assert.Throws<UnauthorizedAccessException>(() => _sandbox.Resolve(absPath));
            Assert.That(ex.Message, Does.Contain("Absolute"));
        }

        [Test]
        public void Resolve_SquadDirectory_Throws()
        {
            var ex = Assert.Throws<UnauthorizedAccessException>(() => _sandbox.Resolve("Squad/part.cfg"));
            Assert.That(ex.Message, Does.Contain("forbidden"));
            Assert.That(ex.Message, Does.Contain("Squad"));
        }

        [Test]
        public void Resolve_SquadExpansionDirectory_Throws()
        {
            var ex = Assert.Throws<UnauthorizedAccessException>(() => _sandbox.Resolve("SquadExpansion/SomeMod/part.cfg"));
            Assert.That(ex.Message, Does.Contain("forbidden"));
        }

        [Test]
        public void Resolve_CustomRestrictedDirectory_Throws()
        {
            var customSandbox = new PathSandbox(_tempRoot, new[] { "Squad", "SquadExpansion", "MyMod" });
            var ex = Assert.Throws<UnauthorizedAccessException>(() => customSandbox.Resolve("MyMod/data.cfg"));
            Assert.That(ex.Message, Does.Contain("forbidden"));
            Assert.That(ex.Message, Does.Contain("MyMod"));
        }

        [Test]
        public void RootPath_ReturnsNormalizedPath()
        {
            Assert.That(_sandbox.RootPath, Is.EqualTo(Path.GetFullPath(_tempRoot)));
        }
    }
}
