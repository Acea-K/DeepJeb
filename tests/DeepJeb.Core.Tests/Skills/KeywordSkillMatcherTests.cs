using System.Collections.Generic;
using DeepJeb.Core.Skills;
using NUnit.Framework;

namespace DeepJeb.Core.Tests.Skills
{
    [TestFixture]
    public class KeywordSkillMatcherTests
    {
        private KeywordSkillMatcher _matcher;
        private List<SkillDefinition> _testSkills;

        [SetUp]
        public void SetUp()
        {
            _matcher = new KeywordSkillMatcher(
                dir => dir == "GameData/kOS" || dir == "GameData/MechJeb2", // dir exists mock
                file => file == "GameData/ModuleManager.4.2.3.dll"          // file exists mock
            );

            _testSkills = new List<SkillDefinition>
            {
                new SkillDefinition
                {
                    Name = "kos-programming",
                    Description = "Comprehensive knowledge base for kOS the programmable scripting mod for KSP",
                    Category = "mods",
                    AppliesWhen = "directory_exists:GameData/kOS",
                    ReferencePaths = new List<string>()
                },
                new SkillDefinition
                {
                    Name = "module-manager",
                    Description = "Knowledge base for Module Manager config file patching tool syntax operators",
                    Category = "mods",
                    AppliesWhen = "file_exists:GameData/ModuleManager.4.2.3.dll",
                    ReferencePaths = new List<string>()
                },
                new SkillDefinition
                {
                    Name = "mechjeb",
                    Description = "Autopilot mod knowledge base for MechJeb ascent guidance landing",
                    Category = "mods",
                    AppliesWhen = "directory_exists:GameData/MechJeb2",
                    ReferencePaths = new List<string>()
                },
                new SkillDefinition
                {
                    Name = "ksp-world-knowledge",
                    Description = "General KSP game mechanics orbital physics spacecraft design gameplay tips",
                    Category = "stock-game",
                    AppliesWhen = "always",
                    ReferencePaths = new List<string>()
                },
                new SkillDefinition
                {
                    Name = "uninstalled-mod",
                    Description = "A skill for a mod that is not installed anywhere",
                    Category = "mods",
                    AppliesWhen = "directory_exists:GameData/DoesNotExist",
                    ReferencePaths = new List<string>()
                }
            };
        }

        private void LoadTestSkills()
        {
            // Hack: inject skills via reflection since we can't use LoadSkills without disk
            typeof(KeywordSkillMatcher)
                .GetField("_skills", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(_matcher, _testSkills);
        }

        [Test]
        public void Match_KosQuestion_HighlyScoresKosSkill()
        {
            LoadTestSkills();
            var results = _matcher.Match("How do I write a launch script in kOS?");

            Assert.That(results.Count, Is.GreaterThan(0));
            // kos-programming should be the top match
            Assert.That(results[0].Skill.Name, Is.EqualTo("kos-programming"));
        }

        [Test]
        public void Match_ModuleManagerQuestion_ScoresMmHighest()
        {
            LoadTestSkills();
            var results = _matcher.Match("How do I patch part configs with Module Manager?");

            Assert.That(results.Count, Is.GreaterThan(0));
            Assert.That(results[0].Skill.Name, Is.EqualTo("module-manager"));
        }

        [Test]
        public void Match_GeneralKspQuestion_ScoresWorldKnowledge()
        {
            LoadTestSkills();
            var results = _matcher.Match("How do I calculate delta-V for a Mun transfer?");

            Assert.That(results.Count, Is.GreaterThan(0));
            Assert.That(results[0].Skill.Name, Is.EqualTo("ksp-world-knowledge"));
        }

        [Test]
        public void Match_UninstalledModDoesNotMatch()
        {
            LoadTestSkills();
            var results = _matcher.Match("How do I use the nonexistent mod?");

            // "uninstalled-mod" should be excluded by condition
            foreach (var r in results)
            {
                Assert.That(r.Skill.Name, Is.Not.EqualTo("uninstalled-mod"));
            }
        }

        [Test]
        public void Match_ReturnsTopNResults()
        {
            LoadTestSkills();
            var results = _matcher.Match("Help with kOS and Module Manager scripting", topN: 2);

            Assert.That(results.Count, Is.LessThanOrEqualTo(2));
        }

        [Test]
        public void Match_EmptyMessage_ReturnsEmptyList()
        {
            LoadTestSkills();
            var results = _matcher.Match("");
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Match_NoSkillsLoaded_ReturnsEmptyList()
        {
            // Don't load skills
            var results = _matcher.Match("anything");
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void EvaluateCondition_Always_ReturnsTrue()
        {
            Assert.That(_matcher.EvaluateCondition("always"), Is.True);
            Assert.That(_matcher.EvaluateCondition(null), Is.True);
            Assert.That(_matcher.EvaluateCondition(""), Is.True);
        }

        [Test]
        public void EvaluateCondition_DirectoryExists_Works()
        {
            Assert.That(_matcher.EvaluateCondition("directory_exists:GameData/kOS"), Is.True);
            Assert.That(_matcher.EvaluateCondition("directory_exists:GameData/DoesNotExist"), Is.False);
        }

        [Test]
        public void EvaluateCondition_FileExists_Works()
        {
            Assert.That(_matcher.EvaluateCondition("file_exists:GameData/ModuleManager.4.2.3.dll"), Is.True);
            Assert.That(_matcher.EvaluateCondition("file_exists:GameData/Nonexistent.dll"), Is.False);
        }

        [Test]
        public void EvaluateCondition_UnknownFormat_ReturnsTrue()
        {
            // Unknown condition formats default to allowing
            Assert.That(_matcher.EvaluateCondition("unknown_condition:something"), Is.True);
        }
    }
}
