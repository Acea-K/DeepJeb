using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DeepJeb.Core.Security
{
    /// <summary>
    /// Tracks soft (threshold-based) keywords across a session.
    /// A single hit is tolerated; 2+ cumulative hits within the same session
    /// trigger a block. Reset between sessions.
    ///
    /// Categories: mild violence, insults, malware references, piracy.
    /// </summary>
    public class SoftKeywordFilter : IInterceptFilter
    {
        public string Name => "SoftKeywordFilter";

        private readonly List<string> _keywords;
        private readonly Regex _regex;
        private readonly HashSet<string> _matchedKeywords;
        private int _hitCount;

        /// <summary>Threshold of cumulative hits before blocking.</summary>
        public int Threshold { get; }

        public SoftKeywordFilter() : this(DefaultKeywords(), 2) { }

        public SoftKeywordFilter(List<string> keywords, int threshold = 2)
        {
            _keywords = keywords ?? new List<string>();
            Threshold = threshold;
            _matchedKeywords = new HashSet<string>();

            if (_keywords.Count > 0)
            {
                var pattern = string.Join("|", _keywords.ConvertAll(Regex.Escape));
                _regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
            else
            {
                _regex = new Regex("(?!)");
            }
        }

        /// <summary>
        /// Inspect content. Returns null on first hit (warning only),
        /// blocks on second+ cumulative hit within the session.
        /// </summary>
        public string Inspect(string content)
        {
            if (string.IsNullOrEmpty(content))
                return null;

            var matches = _regex.Matches(content);
            foreach (Match match in matches)
            {
                string matched = match.Value.ToLowerInvariant();

                // Only count each unique keyword once per session
                if (_matchedKeywords.Add(matched))
                {
                    _hitCount++;
                }
            }

            if (_hitCount >= Threshold)
            {
                return $"Message blocked: sensitive content threshold reached ({_hitCount} hits, threshold={Threshold})";
            }

            return null;
        }

        /// <summary>Reset the session state (call on new session).</summary>
        public void Reset()
        {
            _matchedKeywords.Clear();
            _hitCount = 0;
        }

        public int CurrentHits => _hitCount;

        /// <summary>
        /// Default 30+ soft keywords.
        /// </summary>
        private static List<string> DefaultKeywords()
        {
            return new List<string>
            {
                // Mild violence / fight references
                "punch",
                "stab",
                "shoot",
                "kill",
                "murder",
                "torture",
                "violence",
                "weapon",
                "血腥",
                "暴力",

                // Insults / harassment
                "idiot",
                "stupid",
                "moron",
                "shut up",
                "fuck you",
                "dumb",
                "loser",
                "白痴",
                "傻逼",

                // Malware / hacking references
                "malware",
                "virus creation",
                "ransomware",
                "keylogger",
                "trojan horse",
                "phishing attack",
                "botnet",
                "木马",
                "病毒制作",

                // Piracy / cracking
                "crack serial",
                "keygen",
                "pirated copy",
                "warez",
                "torrent crack",
                "破解补丁",
                "注册机",

                // Self-harm (mild references vs hard filter's instructions)
                "depressed hopeless",
                "want to die",
                "no reason to live",
            };
        }
    }
}
