using System;
using System.Collections.Generic;
using System.Text;

namespace DeepJeb.Unity.Rendering
{
    /// <summary>
    /// Converts Markdown text to Unity IMGUI rich-text lines for display.
    ///
    /// Supports: **bold**, *italic*, `inline code`, #/##/### headings,
    /// -/+/* unordered lists, 1. ordered lists, ```code blocks```,
    /// and | pipe | tables |.
    /// </summary>
    public class MarkdownParser
    {
        public enum LineType { Text, Heading1, Heading2, Heading3, Heading4, CodeBlock, ListItem, OrderedItem, TableRow, TableSeparator }

        public struct MarkdownLine
        {
            public LineType Type;
            public string RichText;    // Unity rich text (with <b>, <i>, <color> tags)
            public string[] TableCells; // Only for TableRow
            public bool IsTableHeader;
        }

        public static List<MarkdownLine> Parse(string markdown)
        {
            var result = new List<MarkdownLine>();
            if (string.IsNullOrEmpty(markdown)) return result;

            var lines = markdown.Replace("\r\n", "\n").Split('\n');
            bool inCodeBlock = false;
            var codeBuilder = new StringBuilder();
            string codeLanguage = null;

            for (int i = 0; i < lines.Length; i++)
            {
                string raw = lines[i];
                string trimmed = raw.Trim();

                // Code block start/end
                if (trimmed.StartsWith("```"))
                {
                    if (inCodeBlock)
                    {
                        // End code block
                        string fixedCode = FixCodeBraces(codeBuilder.ToString(), codeLanguage);
                        result.Add(new MarkdownLine { Type = LineType.CodeBlock, RichText = fixedCode });
                        codeBuilder.Clear();
                        codeLanguage = null;
                        inCodeBlock = false;
                    }
                    else
                    {
                        inCodeBlock = true;
                        codeLanguage = trimmed.Substring(3).Trim().ToLowerInvariant();
                    }
                    continue;
                }

                if (inCodeBlock)
                {
                    codeBuilder.AppendLine(raw);
                    continue;
                }

                // Empty line
                if (string.IsNullOrEmpty(trimmed))
                {
                    result.Add(new MarkdownLine { Type = LineType.Text, RichText = "" });
                    continue;
                }

                // Table
                if (trimmed.StartsWith("|") && trimmed.EndsWith("|"))
                {
                    var cells = ParseTableRow(trimmed);
                    if (IsSeparatorRow(trimmed))
                    {
                        result.Add(new MarkdownLine { Type = LineType.TableSeparator });
                    }
                    else
                    {
                        bool isHeader = i + 1 < lines.Length && IsSeparatorRow(lines[i + 1].Trim());
                        result.Add(new MarkdownLine
                        {
                            Type = LineType.TableRow,
                            TableCells = cells,
                            IsTableHeader = isHeader
                        });
                    }
                    continue;
                }

                // Headings
                if (trimmed.StartsWith("#### "))
                {
                    result.Add(new MarkdownLine { Type = LineType.Heading4, RichText = ParseInline(trimmed.Substring(5)) });
                }
                else if (trimmed.StartsWith("### "))
                {
                    result.Add(new MarkdownLine { Type = LineType.Heading3, RichText = ParseInline(trimmed.Substring(4)) });
                }
                else if (trimmed.StartsWith("## "))
                {
                    result.Add(new MarkdownLine { Type = LineType.Heading2, RichText = ParseInline(trimmed.Substring(3)) });
                }
                else if (trimmed.StartsWith("# "))
                {
                    result.Add(new MarkdownLine { Type = LineType.Heading1, RichText = ParseInline(trimmed.Substring(2)) });
                }
                // Unordered list
                else if (trimmed.StartsWith("- ") || trimmed.StartsWith("* ") || trimmed.StartsWith("+ "))
                {
                    result.Add(new MarkdownLine { Type = LineType.ListItem, RichText = ParseInline(trimmed.Substring(2)) });
                }
                // Ordered list
                else if (System.Text.RegularExpressions.Regex.IsMatch(trimmed, @"^\d+\.\s"))
                {
                    int dotIdx = trimmed.IndexOf(". ");
                    result.Add(new MarkdownLine { Type = LineType.OrderedItem, RichText = ParseInline(trimmed.Substring(dotIdx + 2)) });
                }
                // Regular text
                else
                {
                    result.Add(new MarkdownLine { Type = LineType.Text, RichText = ParseInline(trimmed) });
                }
            }

            // Unclosed code block
            if (inCodeBlock && codeBuilder.Length > 0)
            {
                string fixedCode = FixCodeBraces(codeBuilder.ToString(), codeLanguage);
                result.Add(new MarkdownLine { Type = LineType.CodeBlock, RichText = fixedCode });
            }

            // Trim leading empty lines only — trailing empty lines provide
            // visual separation between messages and must be preserved.
            while (result.Count > 0 && result[0].Type == LineType.Text && string.IsNullOrEmpty(result[0].RichText))
                result.RemoveAt(0);

            return result;
        }

        /// <summary>
        /// Parse inline markdown: **bold**, *italic*, `code`
        /// Converts to Unity IMGUI rich text tags.
        /// </summary>
        private static string ParseInline(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";

            var sb = new StringBuilder();
            int i = 0;
            while (i < text.Length)
            {
                // Inline code
                if (text[i] == '`')
                {
                    int end = text.IndexOf('`', i + 1);
                    if (end > i)
                    {
                        string code = text.Substring(i + 1, end - i - 1);
                        sb.Append("<color=#D9B44D>"); // gold
                        sb.Append(EscapeRichText(code));
                        sb.Append("</color>");
                        i = end + 1;
                        continue;
                    }
                }

                // Bold
                if (i + 1 < text.Length && text[i] == '*' && text[i + 1] == '*')
                {
                    int end = text.IndexOf("**", i + 2);
                    if (end > i)
                    {
                        sb.Append("<b>");
                        sb.Append(EscapeRichText(text.Substring(i + 2, end - i - 2)));
                        sb.Append("</b>");
                        i = end + 2;
                        continue;
                    }
                }

                // Italic
                if (text[i] == '*' && (i == 0 || text[i - 1] != '*'))
                {
                    int end = text.IndexOf('*', i + 1);
                    if (end > i && (end + 1 >= text.Length || text[end + 1] != '*'))
                    {
                        sb.Append("<i>");
                        sb.Append(EscapeRichText(text.Substring(i + 1, end - i - 1)));
                        sb.Append("</i>");
                        i = end + 1;
                        continue;
                    }
                }

                char c = text[i];
                if (c == '<') sb.Append("​<");
                else sb.Append(c);
                i++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Escape characters that have special meaning in Unity rich text.
        /// </summary>
        internal static string EscapeRichText(string text)
        {
            return text.Replace("<", "​<")  // zero-width space BEFORE < breaks tag parsing
                       .Replace(">", ">​");
        }

        // ---- Table helpers ----

        private static string[] ParseTableRow(string line)
        {
            // Remove leading/trailing | and split by |
            string inner = line.Trim();
            if (inner.StartsWith("|")) inner = inner.Substring(1);
            if (inner.EndsWith("|")) inner = inner.Substring(0, inner.Length - 1);
            var cells = inner.Split('|');
            for (int i = 0; i < cells.Length; i++)
                cells[i] = ParseInline(cells[i].Trim());
            return cells;
        }

        private static bool IsSeparatorRow(string line)
        {
            if (string.IsNullOrEmpty(line)) return false;
            var trimmed = line.Trim();
            if (!trimmed.StartsWith("|") || !trimmed.EndsWith("|")) return false;
            // Check that all cells are only - : and spaces
            string inner = trimmed.Substring(1, trimmed.Length - 2);
            foreach (var cell in inner.Split('|'))
            {
                foreach (char c in cell.Trim())
                {
                    if (c != '-' && c != ':' && c != ' ') return false;
                }
            }
            return true;
        }

        // ---- Code block brace auto-completion ----

        private static readonly HashSet<string> BraceLanguages = new HashSet<string>
        {
            "c", "cpp", "c++", "c#", "csharp", "cs", "java", "javascript", "js",
            "typescript", "ts", "rust", "go", "swift", "kotlin", "scala", "dart",
            "kos", "kerboscript", "mm", "modulemanager", "patch", "cfg",
            "py", "python", "python3", "rb", "ruby", "lua"
        };

        private static string FixCodeBraces(string code, string language)
        {
            // Auto-detect brace languages from content when tag is missing
            bool shouldFix = !string.IsNullOrEmpty(language) && BraceLanguages.Contains(language);
            if (!shouldFix && string.IsNullOrEmpty(language))
            {
                // Detect MM patch / KSP config by characteristic operators
                string trimmed = code.TrimStart();
                if (trimmed.Contains("@PART[") || trimmed.Contains("@RESOURCE[") ||
                    trimmed.Contains("HAS[") || trimmed.StartsWith("@"))
                    shouldFix = true;
            }
            if (!shouldFix)
                return code.TrimEnd();

            int openBraces = 0, openBrackets = 0, openParens = 0;

            foreach (char c in code)
            {
                switch (c)
                {
                    case '{': openBraces++; break;
                    case '}': openBraces--; if (openBraces < 0) openBraces = 0; break;
                    case '[': openBrackets++; break;
                    case ']': openBrackets--; if (openBrackets < 0) openBrackets = 0; break;
                    case '(': openParens++; break;
                    case ')': openParens--; if (openParens < 0) openParens = 0; break;
                }
            }

            var sb = new StringBuilder(code.TrimEnd());
            if (openBraces > 0)
            {
                sb.AppendLine();
                for (int i = 0; i < openBraces; i++)
                    sb.Append('}');
            }
            if (openBrackets > 0)
            {
                sb.AppendLine();
                for (int i = 0; i < openBrackets; i++)
                    sb.Append(']');
            }
            if (openParens > 0)
            {
                sb.AppendLine();
                for (int i = 0; i < openParens; i++)
                    sb.Append(')');
            }

            return sb.ToString();
        }
    }
}
