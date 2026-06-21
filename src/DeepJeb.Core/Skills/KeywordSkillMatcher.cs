using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DeepJeb.Core.Skills
{
    /// <summary>
    /// Matches user messages against loaded skills using keyword overlap scoring.
    /// Evaluates applies_when conditions (always, directory_exists, file_exists)
    /// and returns the top-N matching skills.
    /// </summary>
    public class KeywordSkillMatcher : ISkillMatcher
    {
        private readonly List<SkillDefinition> _skills;
        private readonly SkillLoader _loader;
        private readonly Func<string, bool> _directoryExistsChecker;
        private readonly Func<string, bool> _fileExistsChecker;

        public IReadOnlyList<SkillDefinition> LoadedSkills => _skills.AsReadOnly();

        public KeywordSkillMatcher()
            : this(null, null) { }

        public KeywordSkillMatcher(
            Func<string, bool> directoryExistsChecker,
            Func<string, bool> fileExistsChecker)
        {
            _skills = new List<SkillDefinition>();
            _loader = new SkillLoader();
            _directoryExistsChecker = directoryExistsChecker ?? Directory.Exists;
            _fileExistsChecker = fileExistsChecker ?? File.Exists;
        }

        /// <summary>
        /// Load all SKILL.md files from the given directory.
        /// </summary>
        public void LoadSkills(string skillsDirectory)
        {
            var loaded = _loader.LoadFromDirectory(skillsDirectory);
            _skills.Clear();
            _skills.AddRange(loaded);
        }

        /// <summary>
        /// Score all skills against the user message, evaluate conditions,
        /// and return the top N matches.
        /// </summary>
        public List<SkillMatch> Match(string userMessage, int topN = 2)
        {
            if (string.IsNullOrEmpty(userMessage) || _skills.Count == 0)
                return new List<SkillMatch>();

            var candidates = new List<SkillMatch>();

            foreach (var skill in _skills)
            {
                // Check activation condition
                if (!EvaluateCondition(skill.AppliesWhen))
                    continue;

                // Score by keyword overlap between message + skill description
                double score = ComputeScore(userMessage, skill);

                if (score > 0)
                {
                    candidates.Add(new SkillMatch
                    {
                        Skill = skill,
                        Score = score,
                        InjectedReferences = SelectTopReferences(userMessage, skill, 3)
                    });
                }
            }

            // Sort descending by score, take top N
            return candidates
                .OrderByDescending(c => c.Score)
                .Take(Math.Max(1, topN))
                .ToList();
        }

        /// <summary>
        /// Compute a keyword overlap score between the user message and skill metadata.
        /// Uses case-insensitive word-level comparison.
        /// </summary>
        private double ComputeScore(string userMessage, SkillDefinition skill)
        {
            var userWords = Tokenize(userMessage);
            if (userWords.Count == 0) return 0;

            // Combine skill name + description for scoring
            var skillText = (skill.Name + " " + skill.Description).ToLowerInvariant();
            var skillWords = Tokenize(skillText);

            int matches = 0;
            foreach (var uw in userWords)
            {
                if (skillWords.Contains(uw))
                    matches++;
            }

            // Score = fraction of user words that match skill text
            return (double)matches / userWords.Count;
        }

        /// <summary>
        /// Tokenize text into lowercase words (min 2 chars, filter stopwords).
        /// </summary>
        private HashSet<string> Tokenize(string text)
        {
            if (string.IsNullOrEmpty(text)) return new HashSet<string>();

            var words = Regex.Split(text.ToLowerInvariant(), @"\W+");
            var result = new HashSet<string>();

            foreach (var w in words)
            {
                if (w.Length >= 2 && !IsStopword(w))
                    result.Add(w);
            }

            return result;
        }

        private static readonly HashSet<string> Stopwords = new HashSet<string>
        {
            "the", "and", "for", "are", "but", "not", "you", "all",
            "can", "had", "her", "was", "one", "our", "out", "has",
            "have", "been", "some", "that", "this", "with", "from",
            "they", "will", "would", "there", "their", "which", "what",
            "about", "when", "make", "like", "just", "over", "take",
            "into", "your", "its", "than", "then", "now", "also"
        };

        private static bool IsStopword(string word)
        {
            return Stopwords.Contains(word);
        }

        /// <summary>
        /// Evaluate the applies_when condition for a skill.
        /// Supported formats:
        ///   "always" → true
        ///   "directory_exists:Path" → Directory.Exists(path)
        ///   "file_exists:Pattern" → Directory.GetFiles/matching File.Exists
        ///   null/empty → true (always active)
        /// </summary>
        public bool EvaluateCondition(string appliesWhen)
        {
            if (string.IsNullOrEmpty(appliesWhen) || appliesWhen == "always")
                return true;

            if (appliesWhen.StartsWith("directory_exists:"))
            {
                string dirPath = appliesWhen.Substring("directory_exists:".Length).Trim();
                return _directoryExistsChecker(dirPath);
            }

            if (appliesWhen.StartsWith("file_exists:"))
            {
                string filePattern = appliesWhen.Substring("file_exists:".Length).Trim();

                // Check exact file first
                if (_fileExistsChecker(filePattern))
                    return true;

                // If pattern contains wildcard, do glob search
                if (filePattern.Contains("*") || filePattern.Contains("?"))
                {
                    try
                    {
                        string dir = Path.GetDirectoryName(filePattern);
                        string pattern = Path.GetFileName(filePattern);

                        if (!string.IsNullOrEmpty(dir) && _directoryExistsChecker(dir))
                        {
                            var matches = Directory.GetFiles(dir, pattern, SearchOption.TopDirectoryOnly);
                            return matches.Length > 0;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }

                return false;
            }

            // Unknown condition format — allow by default
            return true;
        }

        /// <summary>
        /// Score and select the top N reference files for a matched skill.
        /// </summary>
        private List<string> SelectTopReferences(string userMessage, SkillDefinition skill, int topN)
        {
            if (skill.ReferencePaths == null || skill.ReferencePaths.Count == 0)
                return new List<string>();

            var userWords = Tokenize(userMessage);

            var scored = new List<Tuple<string, double>>();
            foreach (var refPath in skill.ReferencePaths)
            {
                try
                {
                    string refContent = File.ReadAllText(refPath, System.Text.Encoding.UTF8);
                    // Score first 500 chars for efficiency
                    var preview = refContent.Length > 500 ? refContent.Substring(0, 500) : refContent;
                    var refWords = Tokenize(preview);

                    int hits = 0;
                    foreach (var uw in userWords)
                    {
                        if (refWords.Contains(uw))
                            hits++;
                    }

                    double score = userWords.Count > 0 ? (double)hits / userWords.Count : 0;
                    scored.Add(Tuple.Create(refPath, score));
                }
                catch
                {
                    // Skip unreadable references
                }
            }

            return scored
                .OrderByDescending(s => s.Item2)
                .Take(topN)
                .Select(s => s.Item1)
                .ToList();
        }
    }
}
