using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DeepJeb.Core.Agent;
using DeepJeb.Core.Json;

namespace DeepJeb.Unity.Tools
{
    internal static class UrlValidator
    {
        public static string Validate(string url)
        {
            if (string.IsNullOrEmpty(url))
                return "URL is empty.";

            Uri uri;
            try { uri = new Uri(url); }
            catch { return "Invalid URL: " + url; }

            string scheme = uri.Scheme.ToLowerInvariant();
            if (scheme != "http" && scheme != "https")
                return "Only http and https URLs are allowed.";

            string host = uri.Host;
            if (string.IsNullOrEmpty(host))
                return "URL has no host.";

            try
            {
                var addresses = Dns.GetHostAddresses(host);
                if (addresses.Length == 0)
                    return "Could not resolve host: " + host;

                foreach (var addr in addresses)
                {
                    if (IsPrivateOrLoopback(addr))
                        return "Access to private/loopback addresses is blocked: " + addr;
                }
            }
            catch (SocketException)
            {
                return "DNS resolution failed for: " + host;
            }
            catch (Exception ex)
            {
                return "Host validation error: " + ex.Message;
            }

            return null;
        }

        private static bool IsPrivateOrLoopback(IPAddress addr)
        {
            if (IPAddress.IsLoopback(addr)) return true;
            byte[] bytes = addr.GetAddressBytes();
            if (addr.AddressFamily == AddressFamily.InterNetwork)
            {
                if (bytes[0] == 10) return true;
                if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) return true;
                if (bytes[0] == 192 && bytes[1] == 168) return true;
                if (bytes[0] == 127) return true;
                if (bytes[0] == 169 && bytes[1] == 254) return true;
            }
            else if (addr.AddressFamily == AddressFamily.InterNetworkV6)
            {
                if (IPAddress.IPv6Loopback.Equals(addr)) return true;
                if (bytes[0] == 0xFE && (bytes[1] & 0xC0) == 0x80) return true;
                if ((bytes[0] & 0xFE) == 0xFC) return true;
            }
            return false;
        }
    }

    internal static class WebHttpHelper
    {
        static WebHttpHelper()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public static string Get(string url, int timeoutMs, string userAgent, out string contentType)
        {
            contentType = null;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = timeoutMs;
            request.ReadWriteTimeout = timeoutMs;
            request.UserAgent = userAgent ?? "DeepJeb/0.5.5 (KSP Mod)";
            request.AllowAutoRedirect = true;
            request.MaximumAutomaticRedirections = 5;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                contentType = response.ContentType;
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }

    internal class SearchResult
    {
        public string Title;
        public string Snippet;
        public string Url;

        public Dictionary<string, object> ToDict()
        {
            return new Dictionary<string, object>
            {
                ["title"] = Title ?? "",
                ["snippet"] = Snippet ?? "",
                ["url"] = Url ?? ""
            };
        }
    }

    public class WebSearchTool : ITool
    {
        private static DateTime _lastSearchTime = DateTime.MinValue;
        private static readonly TimeSpan Cooldown = TimeSpan.FromSeconds(2);

        private const string BingUA =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
            "(KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

        public string Name => "web_search";
        public string Description =>
            "Search the web using a free search engine. " +
            "Returns up to 10 results with title, snippet, and URL. " +
            "Use this to find current information, documentation, or answers to questions.";

        public string ParametersSchema => @"{
            ""type"": ""object"",
            ""properties"": {
                ""query"": { ""type"": ""string"", ""description"": ""Search query keywords"" },
                ""max_results"": { ""type"": ""integer"", ""description"": ""Max results (1-10, default 5)"" }
            },
            ""required"": [""query""]
        }";

        public async Task<string> ExecuteAsync(string argumentsJson)
        {
            var args = ToolJson.ParseArgs(argumentsJson);
            string query = ToolJson.GetString(args, "query");
            if (string.IsNullOrEmpty(query))
                return ToolJson.Err("Missing parameter: query");

            int maxResults = 5;
            if (args.TryGetValue("max_results", out object mr))
            {
                if (mr is int i) maxResults = i;
                else if (mr is long l) maxResults = (int)l;
                else if (int.TryParse(mr?.ToString(), out int p)) maxResults = p;
            }
            if (maxResults < 1) maxResults = 1;
            if (maxResults > 10) maxResults = 10;

            var now = DateTime.UtcNow;
            if (now - _lastSearchTime < Cooldown)
                return ToolJson.Err("Search rate limit: wait 2 seconds between searches.");
            _lastSearchTime = now;

            var errors = new List<string>();

            // Backend 1: DuckDuckGo Lite
            try
            {
                string ddgUrl = "https://lite.duckduckgo.com/lite/?q=" + Uri.EscapeDataString(query);
                string html = await Task.Run(() =>
                    WebHttpHelper.Get(ddgUrl, 5000, null, out string _));
                var results = ParseDuckDuckGoLite(html, maxResults);
                if (results.Count > 0)
                    return BuildResponse(query, "duckduckgo_lite", results);
                errors.Add("DuckDuckGo returned 0 results");
            }
            catch (Exception ex)
            {
                errors.Add("DuckDuckGo: " + ex.Message);
            }

            // Backend 2: Bing (global)
            try
            {
                string bingUrl = "https://www.bing.com/search?q=" +
                    Uri.EscapeDataString(query) + "&setlang=en";
                string html = await Task.Run(() =>
                    WebHttpHelper.Get(bingUrl, 5000, BingUA, out string _));
                var results = ParseBing(html, maxResults);
                if (results.Count > 0)
                    return BuildResponse(query, "bing", results);
                errors.Add("Bing returned 0 results");
            }
            catch (Exception ex)
            {
                errors.Add("Bing: " + ex.Message);
            }

            // Backend 3: Bing CN
            try
            {
                string bingCnUrl = "https://cn.bing.com/search?q=" +
                    Uri.EscapeDataString(query) + "&setlang=en";
                string html = await Task.Run(() =>
                    WebHttpHelper.Get(bingCnUrl, 5000, BingUA, out string _));
                var results = ParseBing(html, maxResults);
                if (results.Count > 0)
                    return BuildResponse(query, "bing_cn", results);
                errors.Add("Bing CN returned 0 results");
            }
            catch (Exception ex)
            {
                errors.Add("Bing CN: " + ex.Message);
            }

            return ToolJson.Err("All search backends failed: " + string.Join("; ", errors));
        }

        private static string BuildResponse(string query, string backend, List<SearchResult> results)
        {
            var dicts = new List<object>();
            foreach (var r in results)
                dicts.Add(r.ToDict());

            return JsonMapper.Stringify(new Dictionary<string, object>
            {
                ["query"] = query,
                ["backend"] = backend,
                ["result_count"] = results.Count,
                ["results"] = dicts
            });
        }

        private static List<SearchResult> ParseDuckDuckGoLite(string html, int maxResults)
        {
            var results = new List<SearchResult>();
            if (string.IsNullOrEmpty(html)) return results;

            int pos = 0;
            while (results.Count < maxResults)
            {
                int rowStart = html.IndexOf("result-snippet", pos, StringComparison.OrdinalIgnoreCase);
                if (rowStart < 0) break;

                int linkStart = html.IndexOf("<a ", rowStart, StringComparison.OrdinalIgnoreCase);
                if (linkStart < 0 || linkStart - rowStart > 2000) { pos = rowStart + 1; continue; }

                int hrefStart = html.IndexOf("href=\"", linkStart, StringComparison.OrdinalIgnoreCase);
                if (hrefStart < 0 || hrefStart - linkStart > 500) { pos = rowStart + 1; continue; }
                hrefStart += 6;
                int hrefEnd = html.IndexOf('"', hrefStart);
                if (hrefEnd < 0 || hrefEnd - hrefStart > 2000) { pos = rowStart + 1; continue; }
                string url = html.Substring(hrefStart, hrefEnd - hrefStart);

                int tagEnd = html.IndexOf('>', hrefEnd);
                if (tagEnd < 0) { pos = rowStart + 1; continue; }
                int titleEnd = html.IndexOf("</a>", tagEnd, StringComparison.OrdinalIgnoreCase);
                if (titleEnd < 0 || titleEnd - tagEnd > 1000) { pos = rowStart + 1; continue; }
                string title = DecodeHtmlEntities(html.Substring(tagEnd + 1, titleEnd - tagEnd - 1).Trim());

                string snippet = "";
                int snippetTag = html.IndexOf("result-snippet", titleEnd, StringComparison.OrdinalIgnoreCase);
                if (snippetTag >= 0 && snippetTag - titleEnd < 2000)
                {
                    int snipContent = html.IndexOf('>', snippetTag);
                    if (snipContent >= 0)
                    {
                        int snipEnd = html.IndexOf("</span>", snipContent, StringComparison.OrdinalIgnoreCase);
                        if (snipEnd >= 0 && snipEnd - snipContent < 5000)
                            snippet = DecodeHtmlEntities(
                                html.Substring(snipContent + 1, snipEnd - snipContent - 1).Trim());
                    }
                }

                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(url))
                    results.Add(new SearchResult { Title = title, Snippet = snippet, Url = url });

                pos = rowStart + 1;
            }

            return results;
        }

        private static List<SearchResult> ParseBing(string html, int maxResults)
        {
            var results = new List<SearchResult>();
            if (string.IsNullOrEmpty(html)) return results;

            int pos = 0;
            while (results.Count < maxResults)
            {
                int algStart = html.IndexOf("b_algo", pos, StringComparison.OrdinalIgnoreCase);
                if (algStart < 0) break;

                int h2Start = html.IndexOf("<h2>", algStart, StringComparison.OrdinalIgnoreCase);
                if (h2Start < 0 || h2Start - algStart > 3000)
                {
                    int aStart = html.IndexOf("<a ", algStart, StringComparison.OrdinalIgnoreCase);
                    if (aStart < 0 || aStart - algStart > 2000) { pos = algStart + 1; continue; }
                    int hrefStart2 = html.IndexOf("href=\"", aStart, StringComparison.OrdinalIgnoreCase);
                    if (hrefStart2 < 0 || hrefStart2 - aStart > 300) { pos = algStart + 1; continue; }
                    hrefStart2 += 6;
                    int hrefEnd2 = html.IndexOf('"', hrefStart2);
                    if (hrefEnd2 < 0 || hrefEnd2 - hrefStart2 > 2000) { pos = algStart + 1; continue; }
                    string url2 = html.Substring(hrefStart2, hrefEnd2 - hrefStart2);
                    int tagEnd2 = html.IndexOf('>', hrefEnd2);
                    if (tagEnd2 < 0) { pos = algStart + 1; continue; }
                    int titleEnd2 = html.IndexOf("</a>", tagEnd2, StringComparison.OrdinalIgnoreCase);
                    if (titleEnd2 < 0 || titleEnd2 - tagEnd2 > 1000) { pos = algStart + 1; continue; }
                    string title2 = DecodeHtmlEntities(
                        html.Substring(tagEnd2 + 1, titleEnd2 - tagEnd2 - 1).Trim());
                    if (!string.IsNullOrEmpty(title2) && !string.IsNullOrEmpty(url2))
                        results.Add(new SearchResult { Title = title2, Snippet = "", Url = url2 });
                    pos = algStart + 1;
                    continue;
                }

                int aInH2 = html.IndexOf("<a ", h2Start, StringComparison.OrdinalIgnoreCase);
                if (aInH2 < 0 || aInH2 - h2Start > 500) { pos = algStart + 1; continue; }

                int hrefStart = html.IndexOf("href=\"", aInH2, StringComparison.OrdinalIgnoreCase);
                if (hrefStart < 0 || hrefStart - aInH2 > 300) { pos = algStart + 1; continue; }
                hrefStart += 6;
                int hrefEnd = html.IndexOf('"', hrefStart);
                if (hrefEnd < 0 || hrefEnd - hrefStart > 2000) { pos = algStart + 1; continue; }
                string url = html.Substring(hrefStart, hrefEnd - hrefStart);

                int tagEnd = html.IndexOf('>', aInH2);
                if (tagEnd < 0) { pos = algStart + 1; continue; }
                int titleEnd = html.IndexOf("</a>", tagEnd, StringComparison.OrdinalIgnoreCase);
                if (titleEnd < 0 || titleEnd - tagEnd > 1000) { pos = algStart + 1; continue; }
                string title = DecodeHtmlEntities(
                    html.Substring(tagEnd + 1, titleEnd - tagEnd - 1).Trim());

                string snippet = "";
                int pStart = html.IndexOf("<p>", h2Start, StringComparison.OrdinalIgnoreCase);
                if (pStart < 0) pStart = html.IndexOf("<p ", h2Start, StringComparison.OrdinalIgnoreCase);
                if (pStart >= 0 && pStart - h2Start < 5000)
                {
                    int pContent = html.IndexOf('>', pStart);
                    if (pContent >= 0)
                    {
                        int pEnd = html.IndexOf("</p>", pContent, StringComparison.OrdinalIgnoreCase);
                        if (pEnd >= 0 && pEnd - pContent < 5000)
                            snippet = StripHtmlTags(
                                html.Substring(pContent + 1, pEnd - pContent - 1));
                    }
                }

                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(url))
                    results.Add(new SearchResult { Title = title, Snippet = snippet.Trim(), Url = url });

                pos = algStart + 1;
            }

            return results;
        }

        private static string DecodeHtmlEntities(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Replace("&amp;", "&")
                       .Replace("&lt;", "<")
                       .Replace("&gt;", ">")
                       .Replace("&quot;", "\"")
                       .Replace("&#39;", "'")
                       .Replace("&#x27;", "'")
                       .Replace("&nbsp;", " ")
                       .Replace("&#160;", " ");
        }

        private static string StripHtmlTags(string html)
        {
            if (string.IsNullOrEmpty(html)) return html;
            string stripped = Regex.Replace(html, "<[^>]+>", " ");
            stripped = Regex.Replace(stripped, "\\s+", " ").Trim();
            return DecodeHtmlEntities(stripped);
        }
    }

    public class FetchUrlTool : ITool
    {
        private const int MaxResponseBytes = 1 * 1024 * 1024;
        private const int DefaultMaxChars = 8000;
        private const int AbsoluteMaxChars = 50000;
        private const int TimeoutMs = 10000;

        private static readonly HashSet<string> AllowedContentTypes = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase)
        {
            "text/html", "text/plain", "application/json",
            "application/xml", "text/xml", "application/xhtml+xml"
        };

        public string Name => "fetch_url";
        public string Description =>
            "Fetch and read the text content of a web page by URL. " +
            "HTML is stripped to plain text. Max response: 1 MB. " +
            "Use this after web_search to read full pages. " +
            "Private network addresses are blocked.";

        public string ParametersSchema => @"{
            ""type"": ""object"",
            ""properties"": {
                ""url"": { ""type"": ""string"", ""description"": ""Full URL of the page to fetch"" },
                ""max_chars"": { ""type"": ""integer"", ""description"": ""Max characters to return (default 8000, max 50000)"" }
            },
            ""required"": [""url""]
        }";

        public Task<string> ExecuteAsync(string argumentsJson)
        {
            var args = ToolJson.ParseArgs(argumentsJson);
            string url = ToolJson.GetString(args, "url");
            if (string.IsNullOrEmpty(url))
                return Task.FromResult(ToolJson.Err("Missing parameter: url"));

            int maxChars = DefaultMaxChars;
            if (args.TryGetValue("max_chars", out object mc))
            {
                if (mc is int i) maxChars = i;
                else if (mc is long l) maxChars = (int)l;
                else if (int.TryParse(mc?.ToString(), out int p)) maxChars = p;
            }
            if (maxChars < 100) maxChars = 100;
            if (maxChars > AbsoluteMaxChars) maxChars = AbsoluteMaxChars;

            string validationError = UrlValidator.Validate(url);
            if (validationError != null)
                return Task.FromResult(ToolJson.Err(validationError));

            try
            {
                string contentType;
                string body = WebHttpHelper.Get(url, TimeoutMs, null, out contentType);

                string primaryType = contentType ?? "";
                int semicolon = primaryType.IndexOf(';');
                if (semicolon >= 0)
                    primaryType = primaryType.Substring(0, semicolon).Trim();

                if (!AllowedContentTypes.Contains(primaryType))
                    return Task.FromResult(ToolJson.Err(
                        "Content-Type not allowed: " + (contentType ?? "unknown") +
                        ". Only text and JSON are accepted."));

                int byteCount = Encoding.UTF8.GetByteCount(body);
                if (byteCount > MaxResponseBytes)
                    return Task.FromResult(ToolJson.Err(
                        "Response too large: " + byteCount + " bytes (max " + MaxResponseBytes + ")"));

                if (primaryType == "text/html" || primaryType == "application/xhtml+xml")
                {
                    body = StripHtml(body);
                }

                if (body.Length > maxChars)
                    body = body.Substring(0, maxChars) + "\n\n... (truncated)";

                var result = new Dictionary<string, object>
                {
                    ["url"] = url,
                    ["content_type"] = contentType ?? "unknown",
                    ["char_count"] = body.Length,
                    ["text"] = body
                };
                return Task.FromResult(JsonMapper.Stringify(result));
            }
            catch (WebException ex)
            {
                string msg = ex.Message;
                if (ex.Response is HttpWebResponse httpResp)
                    msg = "HTTP " + (int)httpResp.StatusCode + " " + httpResp.StatusDescription;
                return Task.FromResult(ToolJson.Err("Fetch failed: " + msg));
            }
            catch (Exception ex)
            {
                return Task.FromResult(ToolJson.Err("Fetch error: " + ex.Message));
            }
        }

        private static string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) return html;
            html = Regex.Replace(html, "<script[^>]*>.*?</script>", " ",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            html = Regex.Replace(html, "<style[^>]*>.*?</style>", " ",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            html = Regex.Replace(html, "<[^>]+>", " ");
            html = html.Replace("&amp;", "&")
                       .Replace("&lt;", "<")
                       .Replace("&gt;", ">")
                       .Replace("&quot;", "\"")
                       .Replace("&#39;", "'")
                       .Replace("&nbsp;", " ");
            html = Regex.Replace(html, "\\s+", " ").Trim();
            return html;
        }
    }
}
