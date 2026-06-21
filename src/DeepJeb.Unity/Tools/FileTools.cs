using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepJeb.Core.Agent;
using DeepJeb.Core.Json;
using DeepJeb.Core.Security;

namespace DeepJeb.Unity.Tools
{
    /// <summary>Utility for JSON-safe tool responses.</summary>
    internal static class ToolJson
    {
        public static Dictionary<string, object> ParseArgs(string json)
        {
            var parsed = JsonMapper.Parse(json);
            return parsed as Dictionary<string, object> ?? new Dictionary<string, object>();
        }

        public static string GetString(Dictionary<string, object> d, string key)
        {
            return d.TryGetValue(key, out object v) ? v?.ToString() : null;
        }

        public static string Err(string msg) =>
            JsonMapper.Stringify(new Dictionary<string, object> { ["error"] = msg });
        public static string Ok(string key, object val) =>
            JsonMapper.Stringify(new Dictionary<string, object> { [key] = val });

        /// <summary>Keep only the most recent N .bak files for a given file path.</summary>
        public static void PruneOldBackups(string resolvedPath, int keepCount = 3)
        {
            string dir = Path.GetDirectoryName(resolvedPath);
            string name = Path.GetFileName(resolvedPath);
            if (string.IsNullOrEmpty(dir) || string.IsNullOrEmpty(name)) return;

            string prefix = name + ".bak.";
            var backups = Directory.GetFiles(dir, prefix + "*")
                .OrderByDescending(f => f)
                .ToList();

            for (int i = keepCount; i < backups.Count; i++)
            {
                try { File.Delete(backups[i]); } catch { }
            }
        }
    }

    /// <summary>
    /// File-system tool implementations for the AI agent.
    /// All paths are resolved relative to the KSP GameData directory
    /// and validated through PathSandbox.
    /// </summary>

    public class ReadFileTool : ITool
    {
        private readonly PathSandbox _sandbox;
        private const int MaxFileSize = 10 * 1024 * 1024; // 10 MB

        public ReadFileTool(PathSandbox sandbox) { _sandbox = sandbox; }

        public string Name => "read_file";
        public string Description =>
            "Read the contents of a file in GameData. Max file size: 10 MB.";
        public string ParametersSchema => @"{
            ""type"": ""object"",
            ""properties"": {
                ""path"": { ""type"": ""string"", ""description"": ""Relative path from GameData root"" }
            },
            ""required"": [""path""]
        }";

        public Task<string> ExecuteAsync(string argumentsJson)
        {
            var args = ToolJson.ParseArgs(argumentsJson);
            string path = ToolJson.GetString(args, "path");
            if (string.IsNullOrEmpty(path))
                return Task.FromResult("{\"error\":\"Missing parameter: path\"}");

            try
            {
                string resolvedPath = _sandbox.Resolve(path);
                if (!File.Exists(resolvedPath))
                    return Task.FromResult("{\"error\":\"File not found: " + path + "\"}");

                var fileInfo = new FileInfo(resolvedPath);
                if (fileInfo.Length > MaxFileSize)
                    return Task.FromResult("{\"error\":\"File too large: " + fileInfo.Length + " bytes\"}");

                string content = File.ReadAllText(resolvedPath, Encoding.UTF8);
                return Task.FromResult(content);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                return Task.FromResult("{\"error\":\"Access denied: " + ex.Message + "\"}");
            }
            catch (System.Exception ex)
            {
                return Task.FromResult("{\"error\":\"" + ex.Message.Replace("\"", "'") + "\"}");
            }
        }

    }

    public class WriteFileTool : ITool
    {
        private readonly PathSandbox _sandbox;

        public WriteFileTool(PathSandbox sandbox) { _sandbox = sandbox; }

        public string Name => "write_file";
        public string Description =>
            "Write or overwrite a file in GameData. Creates parent directories. " +
            "Previous version backed up as .bak with timestamp.";
        public string ParametersSchema => @"{
            ""type"": ""object"",
            ""properties"": {
                ""path"": { ""type"": ""string"", ""description"": ""Relative path from GameData root"" },
                ""content"": { ""type"": ""string"", ""description"": ""File content to write"" }
            },
            ""required"": [""path"", ""content""]
        }";

        public Task<string> ExecuteAsync(string argumentsJson)
        {
            var args = ToolJson.ParseArgs(argumentsJson);
            string path = ToolJson.GetString(args, "path");
            string content = ToolJson.GetString(args, "content");

            if (string.IsNullOrEmpty(path))
                return Task.FromResult("{\"error\":\"Missing parameter: path\"}");
            if (string.IsNullOrEmpty(content))
                return Task.FromResult("{\"error\":\"Missing parameter: content\"}");

            try
            {
                string resolvedPath = _sandbox.Resolve(path);

                string dir = Path.GetDirectoryName(resolvedPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (File.Exists(resolvedPath))
                {
                    string bakPath = resolvedPath + ".bak." +
                        System.DateTime.Now.ToString("yyyyMMdd-HHmmss");
                    File.Copy(resolvedPath, bakPath, overwrite: true);
                    ToolJson.PruneOldBackups(resolvedPath);
                }

                File.WriteAllText(resolvedPath, content, Encoding.UTF8);

                var result = new Dictionary<string, object>
                {
                    ["success"] = true,
                    ["path"] = path,
                    ["bytes_written"] = Encoding.UTF8.GetByteCount(content)
                };
                return Task.FromResult(JsonMapper.Stringify(result));
            }
            catch (System.UnauthorizedAccessException ex)
            {
                return Task.FromResult("{\"error\":\"Access denied: " + ex.Message + "\"}");
            }
            catch (System.Exception ex)
            {
                return Task.FromResult("{\"error\":\"" + ex.Message.Replace("\"", "'") + "\"}");
            }
        }

    }

    // ---- Additional file tools (per original DeepJeb spec) ----

    public class DeleteFileTool : ITool
    {
        private readonly PathSandbox _sandbox;

        public DeleteFileTool(PathSandbox sandbox) { _sandbox = sandbox; }

        public string Name => "delete_file";
        public string Description =>
            "Delete a file in GameData. Creates a .bak timestamped backup before deletion. " +
            "This action is irreversible — the original file will be permanently removed.";
        public string ParametersSchema => @"{
            ""type"": ""object"",
            ""properties"": {
                ""path"": { ""type"": ""string"", ""description"": ""Relative path from GameData root"" }
            },
            ""required"": [""path""]
        }";

        public Task<string> ExecuteAsync(string argumentsJson)
        {
            var args = ToolJson.ParseArgs(argumentsJson);
            string path = ToolJson.GetString(args, "path");

            if (string.IsNullOrEmpty(path))
                return Task.FromResult("{\"error\":\"Missing parameter: path\"}");

            try
            {
                string resolvedPath = _sandbox.Resolve(path);
                if (!File.Exists(resolvedPath))
                    return Task.FromResult("{\"error\":\"File not found: " + path + "\"}");

                // Create backup before deletion
                string bakPath = resolvedPath + ".bak." + System.DateTime.Now.ToString("yyyyMMdd-HHmmss");
                File.Copy(resolvedPath, bakPath, overwrite: true);
                ToolJson.PruneOldBackups(resolvedPath);
                File.Delete(resolvedPath);

                return Task.FromResult("{\"success\":true,\"path\":\"" + path + "\",\"backup\":\"" + Path.GetFileName(bakPath) + "\"}");
            }
            catch (System.UnauthorizedAccessException ex)
            {
                return Task.FromResult("{\"error\":\"Access denied: " + ex.Message + "\"}");
            }
            catch (System.Exception ex)
            {
                return Task.FromResult("{\"error\":\"" + ex.Message.Replace("\"", "'") + "\"}");
            }
        }
    }

    public class ListDirectoryTool : ITool
    {
        private readonly PathSandbox _sandbox;

        public ListDirectoryTool(PathSandbox sandbox) { _sandbox = sandbox; }

        public string Name => "list_directory";
        public string Description =>
            "List contents of a directory in GameData (single level). " +
            "Returns file names, sizes, and last modified times.";
        public string ParametersSchema => @"{
            ""type"": ""object"",
            ""properties"": {
                ""path"": { ""type"": ""string"", ""description"": ""Relative directory path from GameData root"" },
                ""filter"": { ""type"": ""string"", ""description"": ""Optional wildcard filter (e.g. '*.cfg')"" }
            },
            ""required"": [""path""]
        }";

        public Task<string> ExecuteAsync(string argumentsJson)
        {
            var args = ToolJson.ParseArgs(argumentsJson);
            string path = ToolJson.GetString(args, "path");
            string filter = ToolJson.GetString(args, "filter") ?? "*";

            if (string.IsNullOrEmpty(path))
                return Task.FromResult("{\"error\":\"Missing parameter: path\"}");

            try
            {
                string resolvedPath = _sandbox.Resolve(path);
                if (!Directory.Exists(resolvedPath))
                    return Task.FromResult("{\"error\":\"Directory not found: " + path + "\"}");

                var entries = new List<object>();
                foreach (var file in Directory.GetFiles(resolvedPath, filter, SearchOption.TopDirectoryOnly))
                {
                    var info = new FileInfo(file);
                    entries.Add(new Dictionary<string, object>
                    {
                        ["name"] = info.Name,
                        ["size"] = info.Length,
                        ["modified"] = info.LastWriteTimeUtc.ToString("o")
                    });
                }
                foreach (var dir in Directory.GetDirectories(resolvedPath))
                {
                    entries.Add(new Dictionary<string, object>
                    {
                        ["name"] = Path.GetFileName(dir) + "/",
                        ["type"] = "directory"
                    });
                }

                return Task.FromResult(JsonMapper.Stringify(new Dictionary<string, object>
                {
                    ["path"] = path,
                    ["entries"] = entries,
                    ["count"] = entries.Count
                }));
            }
            catch (System.UnauthorizedAccessException ex)
            {
                return Task.FromResult("{\"error\":\"Access denied: " + ex.Message + "\"}");
            }
            catch (System.Exception ex)
            {
                return Task.FromResult("{\"error\":\"" + ex.Message.Replace("\"", "'") + "\"}");
            }
        }
    }

    public class FileExistsTool : ITool
    {
        private readonly PathSandbox _sandbox;

        public FileExistsTool(PathSandbox sandbox) { _sandbox = sandbox; }

        public string Name => "file_exists";
        public string Description =>
            "Check if a file or directory exists in GameData. Returns true or false.";
        public string ParametersSchema => @"{
            ""type"": ""object"",
            ""properties"": {
                ""path"": { ""type"": ""string"", ""description"": ""Relative path from GameData root"" },
                ""type"": { ""type"": ""string"", ""description"": ""'file' or 'directory' (default: 'file')"" }
            },
            ""required"": [""path""]
        }";

        public Task<string> ExecuteAsync(string argumentsJson)
        {
            var args = ToolJson.ParseArgs(argumentsJson);
            string path = ToolJson.GetString(args, "path");
            string type = ToolJson.GetString(args, "type") ?? "file";

            if (string.IsNullOrEmpty(path))
                return Task.FromResult("{\"error\":\"Missing parameter: path\"}");

            try
            {
                string resolvedPath = _sandbox.Resolve(path);
                bool exists = type == "directory" ? Directory.Exists(resolvedPath) : File.Exists(resolvedPath);
                return Task.FromResult("{\"exists\":" + exists.ToString().ToLowerInvariant() + ",\"path\":\"" + path + "\"}");
            }
            catch (System.UnauthorizedAccessException)
            {
                return Task.FromResult("{\"exists\":false,\"path\":\"" + path + "\",\"error\":\"Access denied\"}");
            }
            catch (System.Exception ex)
            {
                return Task.FromResult("{\"exists\":false,\"path\":\"" + path + "\",\"error\":\"" + ex.Message.Replace("\"", "'") + "\"}");
            }
        }
    }

    public class BackupFileTool : ITool
    {
        private readonly PathSandbox _sandbox;

        public BackupFileTool(PathSandbox sandbox) { _sandbox = sandbox; }

        public string Name => "backup_file";
        public string Description =>
            "Create a timestamped .bak backup of a file in GameData without modifying the original.";
        public string ParametersSchema => @"{
            ""type"": ""object"",
            ""properties"": {
                ""path"": { ""type"": ""string"", ""description"": ""Relative path from GameData root"" }
            },
            ""required"": [""path""]
        }";

        public Task<string> ExecuteAsync(string argumentsJson)
        {
            var args = ToolJson.ParseArgs(argumentsJson);
            string path = ToolJson.GetString(args, "path");

            if (string.IsNullOrEmpty(path))
                return Task.FromResult("{\"error\":\"Missing parameter: path\"}");

            try
            {
                string resolvedPath = _sandbox.Resolve(path);
                if (!File.Exists(resolvedPath))
                    return Task.FromResult("{\"error\":\"File not found: " + path + "\"}");

                string bakPath = resolvedPath + ".bak." + System.DateTime.Now.ToString("yyyyMMdd-HHmmss");
                File.Copy(resolvedPath, bakPath, overwrite: true);
                ToolJson.PruneOldBackups(resolvedPath);

                return Task.FromResult("{\"success\":true,\"original\":\"" + path + "\",\"backup\":\"" + Path.GetFileName(bakPath) + "\"}");
            }
            catch (System.UnauthorizedAccessException ex)
            {
                return Task.FromResult("{\"error\":\"Access denied: " + ex.Message + "\"}");
            }
            catch (System.Exception ex)
            {
                return Task.FromResult("{\"error\":\"" + ex.Message.Replace("\"", "'") + "\"}");
            }
        }
    }
}
