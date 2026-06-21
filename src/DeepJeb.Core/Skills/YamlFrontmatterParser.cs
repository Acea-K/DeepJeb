using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DeepJeb.Core.Skills
{
    /// <summary>
    /// Lightweight YAML frontmatter parser for SKILL.md files.
    /// Extracts the YAML block between --- delimiters and the body that follows.
    /// Supports only the subset of YAML needed for skill frontmatter:
    /// simple scalars, single-level lists, and multi-line strings (via >).
    /// </summary>
    public class YamlFrontmatterParser
    {
        /// <summary>
        /// Parse a SKILL.md file, returning the frontmatter as key-value pairs
        /// and the body as the raw markdown after the closing ---.
        /// </summary>
        public ParseResult Parse(string fileContent)
        {
            if (string.IsNullOrEmpty(fileContent))
                return new ParseResult { Body = string.Empty };

            var lines = fileContent.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            // Find the opening ---
            int openIndex = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim() == "---")
                {
                    openIndex = i;
                    break;
                }
            }

            if (openIndex < 0)
            {
                // No frontmatter — treat entire file as body
                return new ParseResult { Body = fileContent };
            }

            // Find the closing ---
            int closeIndex = -1;
            for (int i = openIndex + 1; i < lines.Length; i++)
            {
                if (lines[i].Trim() == "---")
                {
                    closeIndex = i;
                    break;
                }
            }

            if (closeIndex < 0)
            {
                // Opening --- found but no closing — treat as body
                return new ParseResult { Body = fileContent };
            }

            // Extract frontmatter lines (between openIndex+1 and closeIndex-1)
            var frontmatterLines = new string[closeIndex - openIndex - 1];
            Array.Copy(lines, openIndex + 1, frontmatterLines, 0, frontmatterLines.Length);

            var frontmatter = ParseYamlBlock(frontmatterLines);

            // Extract body (everything after closeIndex)
            var bodyLines = new string[lines.Length - closeIndex - 1];
            Array.Copy(lines, closeIndex + 1, bodyLines, 0, bodyLines.Length);
            // Trim leading blank lines from body
            var body = string.Join("\n", bodyLines).TrimStart('\n', '\r');

            return new ParseResult
            {
                Frontmatter = frontmatter,
                Body = body
            };
        }

        /// <summary>
        /// Parse a simple YAML key-value block.
        /// Supports: scalars, folded scalars (>), and single-level list items (-).
        /// </summary>
        private Dictionary<string, object> ParseYamlBlock(string[] lines)
        {
            var result = new Dictionary<string, object>();
            string currentKey = null;
            var currentList = new List<string>();
            var foldedLines = new List<string>();
            bool inFolded = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string trimmed = line.TrimStart();

                // Skip empty lines and comments
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                {
                    // Empty line after folded scalar ends it
                    if (inFolded && string.IsNullOrWhiteSpace(trimmed))
                    {
                        if (currentKey != null)
                        {
                            result[currentKey] = string.Join(" ", foldedLines);
                            foldedLines.Clear();
                            currentKey = null;
                            inFolded = false;
                        }
                    }
                    continue;
                }

                // List item (- value)
                if (trimmed.StartsWith("- ") || trimmed == "-")
                {
                    string value = trimmed.Length > 2 ? trimmed.Substring(2).Trim() : string.Empty;
                    // Remove surrounding quotes
                    value = StripQuotes(value);
                    currentList.Add(value);
                    continue;
                }

                // Check for key: value
                int colonIndex = trimmed.IndexOf(':');
                if (colonIndex > 0 && !trimmed.StartsWith("-"))
                {
                    // Flush any accumulated list
                    if (currentKey != null && currentList.Count > 0)
                    {
                        result[currentKey] = new List<string>(currentList);
                        currentList.Clear();
                    }
                    // Flush folded scalar
                    if (inFolded && currentKey != null)
                    {
                        result[currentKey] = string.Join(" ", foldedLines);
                        foldedLines.Clear();
                        inFolded = false;
                    }

                    currentKey = trimmed.Substring(0, colonIndex).Trim();
                    string rawValue = trimmed.Substring(colonIndex + 1).Trim();

                    if (string.IsNullOrEmpty(rawValue) || rawValue == ">" || rawValue == "|")
                    {
                        // Folded or literal block scalar
                        if (rawValue == ">" || rawValue == "|")
                        {
                            inFolded = true;
                        }
                        // Key with no value on this line — could be start of a list or empty
                        continue;
                    }

                    // Simple scalar value
                    result[currentKey] = StripQuotes(rawValue);
                    currentKey = null;
                }
                else if (inFolded && currentKey != null)
                {
                    // Continuation line of a folded scalar
                    foldedLines.Add(trimmed);
                }
            }

            // Flush remaining state
            if (currentKey != null && currentList.Count > 0)
            {
                result[currentKey] = new List<string>(currentList);
            }
            if (inFolded && currentKey != null)
            {
                result[currentKey] = string.Join(" ", foldedLines);
            }

            return result;
        }

        private static string StripQuotes(string value)
        {
            if (value == null) return null;
            if (value.Length >= 2 &&
                ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                 (value.StartsWith("'") && value.EndsWith("'"))))
            {
                return value.Substring(1, value.Length - 2);
            }
            return value;
        }

        /// <summary>
        /// Get a string value from the frontmatter dictionary, or null.
        /// </summary>
        public static string GetString(Dictionary<string, object> fm, string key)
        {
            if (fm.TryGetValue(key, out var value) && value is string s)
                return s;
            return null;
        }

        /// <summary>
        /// Get a list of strings from the frontmatter dictionary, or empty list.
        /// </summary>
        public static List<string> GetStringList(Dictionary<string, object> fm, string key)
        {
            if (fm.TryGetValue(key, out var value))
            {
                if (value is List<string> list)
                    return list;
                if (value is string s)
                    return new List<string> { s };
            }
            return new List<string>();
        }
    }

    public class ParseResult
    {
        public Dictionary<string, object> Frontmatter { get; set; }
        public string Body { get; set; }

        public ParseResult()
        {
            Frontmatter = new Dictionary<string, object>();
        }
    }
}
