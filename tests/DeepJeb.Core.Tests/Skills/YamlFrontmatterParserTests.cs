using DeepJeb.Core.Skills;
using NUnit.Framework;

namespace DeepJeb.Core.Tests.Skills
{
    [TestFixture]
    public class YamlFrontmatterParserTests
    {
        private YamlFrontmatterParser _parser;

        [SetUp]
        public void SetUp()
        {
            _parser = new YamlFrontmatterParser();
        }

        [Test]
        public void Parse_ValidFrontmatter_ExtractsFields()
        {
            var input = "---\nname: test-skill\ndescription: A test skill\ncategory: mods\n---\n\n# Body\n\nThis is the body.";
            var result = _parser.Parse(input);

            Assert.That(result.Frontmatter.Count, Is.GreaterThan(0));
            Assert.That(YamlFrontmatterParser.GetString(result.Frontmatter, "name"), Is.EqualTo("test-skill"));
            Assert.That(YamlFrontmatterParser.GetString(result.Frontmatter, "description"), Is.EqualTo("A test skill"));
            Assert.That(YamlFrontmatterParser.GetString(result.Frontmatter, "category"), Is.EqualTo("mods"));
            Assert.That(result.Body, Does.Contain("This is the body"));
        }

        [Test]
        public void Parse_NoFrontmatter_TreatsAllAsBody()
        {
            var input = "# Just a markdown file\n\nNo frontmatter here.";
            var result = _parser.Parse(input);

            Assert.That(result.Frontmatter.Count, Is.EqualTo(0));
            Assert.That(result.Body, Is.EqualTo(input));
        }

        [Test]
        public void Parse_EmptyString_ReturnsEmptyBody()
        {
            var result = _parser.Parse("");
            Assert.That(result.Body, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Parse_Null_ReturnsEmptyBody()
        {
            var result = _parser.Parse(null);
            Assert.That(result.Body, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Parse_UnclosedFrontmatter_TreatsAsBody()
        {
            var input = "---\nname: broken\n";
            var result = _parser.Parse(input);
            Assert.That(result.Body, Is.EqualTo(input));
        }

        [Test]
        public void Parse_QuotedStrings_StripsQuotes()
        {
            var input = "---\nname: \"quoted-value\"\ndescription: 'single-quoted'\n---\n\nBody";
            var result = _parser.Parse(input);

            Assert.That(YamlFrontmatterParser.GetString(result.Frontmatter, "name"), Is.EqualTo("quoted-value"));
            Assert.That(YamlFrontmatterParser.GetString(result.Frontmatter, "description"), Is.EqualTo("single-quoted"));
        }

        [Test]
        public void Parse_ListField_ExtractsAsList()
        {
            var input = "---\nname: skill-with-sources\nsources:\n  - https://example.com\n  - https://wiki.com\n---\n\nBody";
            var result = _parser.Parse(input);

            var sources = YamlFrontmatterParser.GetStringList(result.Frontmatter, "sources");
            Assert.That(sources, Is.Not.Null);
            Assert.That(sources.Count, Is.EqualTo(2));
            Assert.That(sources[0], Is.EqualTo("https://example.com"));
            Assert.That(sources[1], Is.EqualTo("https://wiki.com"));
        }

        [Test]
        public void Parse_FoldedScalar_JoinsLines()
        {
            var input = "---\nname: folded-test\ndescription: >\n  This is a long description\n  that spans multiple lines.\n---\n\nBody";
            var result = _parser.Parse(input);

            var desc = YamlFrontmatterParser.GetString(result.Frontmatter, "description");
            Assert.That(desc, Does.Contain("This is a long description"));
            Assert.That(desc, Does.Contain("that spans multiple lines"));
        }

        [Test]
        public void Parse_CommentLines_Ignored()
        {
            var input = "---\n# This is a comment\nname: real-skill\n# Another comment\ncategory: mods\n---\n\nBody";
            var result = _parser.Parse(input);

            Assert.That(YamlFrontmatterParser.GetString(result.Frontmatter, "name"), Is.EqualTo("real-skill"));
            Assert.That(YamlFrontmatterParser.GetString(result.Frontmatter, "category"), Is.EqualTo("mods"));
        }

        [Test]
        public void GetString_MissingKey_ReturnsNull()
        {
            var input = "---\nname: test\n---\n\nBody";
            var result = _parser.Parse(input);

            Assert.That(YamlFrontmatterParser.GetString(result.Frontmatter, "nonexistent"), Is.Null);
        }

        [Test]
        public void GetStringList_SingleString_WrapsInList()
        {
            var input = "---\nname: test\ndescription: just one line\n---\n\nBody";
            var result = _parser.Parse(input);

            var desc = YamlFrontmatterParser.GetStringList(result.Frontmatter, "description");
            Assert.That(desc.Count, Is.EqualTo(1));
            Assert.That(desc[0], Is.EqualTo("just one line"));
        }

        [Test]
        public void Parse_AppliesWhenField_Works()
        {
            var input = "---\nname: conditional-skill\napplies_when: \"directory_exists:GameData/kOS\"\n---\n\nBody";
            var result = _parser.Parse(input);

            Assert.That(YamlFrontmatterParser.GetString(result.Frontmatter, "applies_when"),
                Is.EqualTo("directory_exists:GameData/kOS"));
        }

        [Test]
        public void Parse_RealWorldKosSkill_HasAllFields()
        {
            // Simulate the real SKILL.md structure from GameData/Skills/mods/kos-programming/SKILL.md
            var input = @"---
name: kos-programming
description: >
  Comprehensive knowledge base for kOS (Kerbal Operating System).
author: Acea
sources:
  - https://ksp-kos.github.io/KOS/
category: mods
version: 1.0
last_updated: 2026-06-17
applies_when: ""directory_exists:GameData/kOS""
---

# kOS Programming

Knowledge body content here.";

            var result = _parser.Parse(input);

            Assert.That(YamlFrontmatterParser.GetString(result.Frontmatter, "name"), Is.EqualTo("kos-programming"));
            Assert.That(YamlFrontmatterParser.GetString(result.Frontmatter, "author"), Is.EqualTo("Acea"));
            Assert.That(YamlFrontmatterParser.GetString(result.Frontmatter, "version"), Is.EqualTo("1.0"));
            Assert.That(YamlFrontmatterParser.GetString(result.Frontmatter, "last_updated"), Is.EqualTo("2026-06-17"));
            Assert.That(YamlFrontmatterParser.GetString(result.Frontmatter, "applies_when"), Is.EqualTo("directory_exists:GameData/kOS"));

            var sources = YamlFrontmatterParser.GetStringList(result.Frontmatter, "sources");
            Assert.That(sources.Count, Is.EqualTo(1));
            Assert.That(sources[0], Is.EqualTo("https://ksp-kos.github.io/KOS/"));

            Assert.That(result.Body, Does.Contain("# kOS Programming"));
            Assert.That(result.Body, Does.Contain("Knowledge body content here"));
        }
    }
}
