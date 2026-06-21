using System;
using System.IO;

namespace DeepJeb.Core.Security
{
    /// <summary>
    /// Ensures all file-system operations stay within allowed directories.
    /// Rejects absolute paths outside the sandbox, ".." traversal, and
    /// paths targeting restricted directories (e.g. Squad/, SquadExpansion/).
    /// </summary>
    public class PathSandbox
    {
        private readonly string _rootPath;
        private readonly string[] _restrictedDirectories;

        public PathSandbox(string rootPath, string[] restrictedDirectories = null)
        {
            _rootPath = Path.GetFullPath(rootPath);
            _restrictedDirectories = restrictedDirectories ?? new[] { "Squad", "SquadExpansion" };
        }

        /// <summary>
        /// Validate and resolve a relative path within the sandbox.
        /// Returns the resolved absolute path, or throws if the path escapes.
        /// </summary>
        public string Resolve(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                throw new ArgumentException("Path cannot be empty.");

            // Reject attempts to use absolute paths or traversal
            if (Path.IsPathRooted(relativePath))
                throw new UnauthorizedAccessException("Absolute paths are not allowed.");

            if (relativePath == "~" || relativePath.StartsWith("~/") || relativePath.StartsWith("~\\"))
                throw new UnauthorizedAccessException("Home directory references are not allowed.");

            string fullPath = Path.GetFullPath(Path.Combine(_rootPath, relativePath));

            // Must stay within root — check with directory separator to prevent
            // same-prefix bypass (e.g. GameData vs GameDataExtra).
            string normalizedRoot = _rootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (!fullPath.StartsWith(normalizedRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                && !fullPath.Equals(normalizedRoot, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Path escapes the sandbox root.");

            // Check restricted directories
            foreach (var restricted in _restrictedDirectories)
            {
                string restrictedPath = Path.Combine(_rootPath, restricted);
                string restrictedWithSep = restrictedPath + Path.DirectorySeparatorChar;
                if (fullPath.StartsWith(restrictedWithSep, StringComparison.OrdinalIgnoreCase)
                    || fullPath.Equals(restrictedPath, StringComparison.OrdinalIgnoreCase))
                    throw new UnauthorizedAccessException($"Access to '{restricted}' directory is forbidden.");
            }

            return fullPath;
        }

        public string RootPath => _rootPath;
    }
}
