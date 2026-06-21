using System.Collections.Generic;

namespace DeepJeb.Core.Skills
{
    /// <summary>
    /// Matches user messages against loaded skill definitions using
    /// keyword scoring. Returns the top-N matching skills for injection
    /// into the system prompt.
    ///
    /// Skills are stored as standard Claude Skill documents: SKILL.md files
    /// with YAML frontmatter + Markdown body, organized in
    /// GameData/DeepJeb/Skills/{category}/{skill-name}/SKILL.md with optional
    /// references/ subdirectories for supplementary knowledge.
    /// </summary>
    public interface ISkillMatcher
    {
        /// <summary>Load all SKILL.md files from the given directory tree.</summary>
        void LoadSkills(string skillsDirectory);

        /// <summary>
        /// Score all loaded skills against the user's message.
        /// Returns the top N matches (default 2), respecting
        /// conditional activation rules (applies_when field).
        /// </summary>
        List<SkillMatch> Match(string userMessage, int topN = 2);

        /// <summary>All loaded skills, regardless of activation state.</summary>
        IReadOnlyList<SkillDefinition> LoadedSkills { get; }
    }

    /// <summary>
    /// Parsed from a SKILL.md file's YAML frontmatter + Markdown body.
    /// Follows the standard Claude Skill document format.
    /// </summary>
    public class SkillDefinition
    {
        /// <summary>Unique skill identifier (e.g. "kos-programming").</summary>
        public string Name { get; set; }

        /// <summary>Human-readable description used for keyword matching.</summary>
        public string Description { get; set; }

        /// <summary>Author of the skill document.</summary>
        public string Author { get; set; }

        /// <summary>Source URLs this knowledge was compiled from.</summary>
        public List<string> Sources { get; set; }

        /// <summary>Category folder (e.g. "mods", "stock-game").</summary>
        public string Category { get; set; }

        /// <summary>Skill version string.</summary>
        public string Version { get; set; }

        /// <summary>Last update date (ISO 8601).</summary>
        public string LastUpdated { get; set; }

        /// <summary>
        /// Activation condition. Supported formats:
        ///   "always" — always active
        ///   "directory_exists:GameData/ModName" — active when directory exists
        ///   "file_exists:GameData/ModName*.dll" — active when matching file exists
        /// </summary>
        public string AppliesWhen { get; set; }

        /// <summary>The Markdown body (everything after the YAML frontmatter).</summary>
        public string Body { get; set; }

        /// <summary>Absolute path to the SKILL.md file on disk.</summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Paths to supplementary reference .md files in the references/ directory,
        /// injected into the system prompt when this skill matches (up to 3).
        /// </summary>
        public List<string> ReferencePaths { get; set; }
    }

    /// <summary>
    /// A single skill match result, including the matched skill,
    /// its relevance score, and any reference files to inject.
    /// </summary>
    public class SkillMatch
    {
        public SkillDefinition Skill { get; set; }
        public double Score { get; set; }
        public List<string> InjectedReferences { get; set; }
    }
}
