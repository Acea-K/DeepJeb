using System;
using System.Collections.Generic;
using System.IO;

namespace DeepJeb.Core.Skills
{
    /// <summary>
    /// Scans the skills directory tree for SKILL.md files,
    /// parses them into SkillDefinition objects, and collects
    /// reference paths from the references/ subdirectory.
    /// </summary>
    public class SkillLoader
    {
        private readonly YamlFrontmatterParser _parser;

        public SkillLoader()
        {
            _parser = new YamlFrontmatterParser();
        }

        /// <summary>
        /// Load all SKILL.md files from the given root directory.
        /// Recursively scans subdirectories. Each skill is in its own
        /// subfolder: {root}/{category}/{skill-name}/SKILL.md
        /// with optional {root}/{category}/{skill-name}/references/*.md
        /// </summary>
        public List<SkillDefinition> LoadFromDirectory(string skillsRoot)
        {
            var skills = new List<SkillDefinition>();

            if (!Directory.Exists(skillsRoot))
                return skills;

            var skillFiles = Directory.GetFiles(skillsRoot, "SKILL.md", SearchOption.AllDirectories);

            foreach (var filePath in skillFiles)
            {
                try
                {
                    var skill = LoadSkillFile(filePath, skillsRoot);
                    if (skill != null)
                        skills.Add(skill);
                }
                catch
                {
                    // Skip malformed skill files — log in production
                }
            }

            return skills;
        }

        private SkillDefinition LoadSkillFile(string filePath, string skillsRoot)
        {
            string content = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            var parseResult = _parser.Parse(content);

            var skill = new SkillDefinition
            {
                Name = YamlFrontmatterParser.GetString(parseResult.Frontmatter, "name") ?? "unknown",
                Description = YamlFrontmatterParser.GetString(parseResult.Frontmatter, "description") ?? string.Empty,
                Author = YamlFrontmatterParser.GetString(parseResult.Frontmatter, "author"),
                Category = YamlFrontmatterParser.GetString(parseResult.Frontmatter, "category"),
                Version = YamlFrontmatterParser.GetString(parseResult.Frontmatter, "version"),
                LastUpdated = YamlFrontmatterParser.GetString(parseResult.Frontmatter, "last_updated"),
                AppliesWhen = YamlFrontmatterParser.GetString(parseResult.Frontmatter, "applies_when") ?? "always",
                Sources = YamlFrontmatterParser.GetStringList(parseResult.Frontmatter, "sources"),
                Body = parseResult.Body,
                FilePath = filePath
            };

            // Derive category from directory structure if not in frontmatter
            if (string.IsNullOrEmpty(skill.Category))
            {
                skill.Category = DeriveCategory(filePath, skillsRoot);
            }

            // Collect reference files
            var skillDir = Path.GetDirectoryName(filePath);
            var refDir = Path.Combine(skillDir, "references");
            if (Directory.Exists(refDir))
            {
                skill.ReferencePaths = new List<string>(
                    Directory.GetFiles(refDir, "*.md", SearchOption.TopDirectoryOnly));
            }
            else
            {
                skill.ReferencePaths = new List<string>();
            }

            return skill;
        }

        private static string DeriveCategory(string filePath, string skillsRoot)
        {
            // filePath:  .../GameData/DeepJeb/Skills/mods/kos-programming/SKILL.md
            // skillsRoot: .../GameData/DeepJeb/Skills
            // We want: "mods"
            var skillDir = Path.GetDirectoryName(filePath); // .../Skills/mods/kos-programming
            if (skillDir == null) return "uncategorized";

            var parentDir = Path.GetDirectoryName(skillDir); // .../Skills/mods
            if (parentDir == null) return "uncategorized";

            // Normalize to get relative path from skillsRoot
            string normalizedSkillRoot = Path.GetFullPath(skillsRoot).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string normalizedParent = Path.GetFullPath(parentDir).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            if (normalizedParent.Equals(normalizedSkillRoot, StringComparison.OrdinalIgnoreCase))
            {
                // The skill is directly under root: Skills/skill-name/SKILL.md
                return "root";
            }

            // Return the directory name under root
            return Path.GetFileName(parentDir);
        }
    }
}
