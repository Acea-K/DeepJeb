using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeepJeb.Protocol
{
    /// <summary>
    /// Shared HTTP utility for API clients.
    /// All connections use TLS 1.2 as required by DeepJeb security policy.
    /// </summary>
    public static class HttpHelper
    {
        static HttpHelper()
        {
            // Enforce TLS 1.2 for all connections (KSP .NET 4.6 default is TLS 1.0)
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Perform a GET request with an optional Bearer token.
        /// Returns the response body as a string.
        /// </summary>
        public static string Get(string url, string apiKey, int timeoutMs = 15000, Dictionary<string, string> extraHeaders = null)
        {
            var request = CreateRequest(url, "GET", apiKey, timeoutMs, extraHeaders);
            return ReadResponse(request);
        }

        /// <summary>
        /// Perform a POST request with a JSON body and optional Bearer token.
        /// Returns the response body as a string.
        /// </summary>
        public static string Post(string url, string jsonBody, string apiKey, int timeoutMs = 60000, Dictionary<string, string> extraHeaders = null)
        {
            var request = CreateRequest(url, "POST", apiKey, timeoutMs, extraHeaders);
            request.ContentType = "application/json; charset=utf-8";

            byte[] bodyBytes = Encoding.UTF8.GetBytes(jsonBody ?? "");
            request.ContentLength = bodyBytes.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(bodyBytes, 0, bodyBytes.Length);
            }

            return ReadResponse(request);
        }

        /// <summary>
        /// POST with streaming response. Calls onLine for each SSE data line.
        /// Uses BeginGetResponse for async reading on .NET 4.6.
        /// </summary>
        public static void PostStreaming(
            string url,
            string jsonBody,
            string apiKey,
            Action<string> onLine,
            Action onComplete,
            Action<string> onError,
            int timeoutMs = 120000,
            Dictionary<string, string> extraHeaders = null,
            Action<HttpWebRequest> onRequestCreated = null)
        {
            try
            {
                var request = CreateRequest(url, "POST", apiKey, timeoutMs, extraHeaders);
                onRequestCreated?.Invoke(request);
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "text/event-stream";

                byte[] bodyBytes = Encoding.UTF8.GetBytes(jsonBody ?? "");
                request.ContentLength = bodyBytes.Length;

                using (var reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bodyBytes, 0, bodyBytes.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var respStream = response.GetResponseStream())
                using (var reader = new StreamReader(respStream, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (line == null) break;
                        onLine(line);
                    }
                }

                onComplete();
            }
            catch (Exception ex)
            {
                onError(ex.Message);
            }
        }

        /// <summary>
        /// Test connectivity by making a GET request. Returns true for 2xx.
        /// </summary>
        public static bool TestConnection(string url, string apiKey, int timeoutMs = 10000, Dictionary<string, string> extraHeaders = null)
        {
            try
            {
                var request = CreateRequest(url, "GET", apiKey, timeoutMs, extraHeaders);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return (int)response.StatusCode >= 200 && (int)response.StatusCode < 300;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Extra headers for this helper instance. Use per-request extraHeaders for provider-specific headers.</summary>
        [Obsolete("Pass extraHeaders directly to each method instead.")]
        public static Dictionary<string, string> ExtraHeaders { get; set; } = new Dictionary<string, string>();

        private static HttpWebRequest CreateRequest(string url, string method, string apiKey, int timeoutMs, Dictionary<string, string> extraHeaders = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.Timeout = timeoutMs;
            request.ReadWriteTimeout = timeoutMs;
            request.UserAgent = "DeepJeb/0.5.3 (KSP Mod)";

            if (!string.IsNullOrEmpty(apiKey))
            {
                request.Headers["Authorization"] = "Bearer " + apiKey;
            }

            if (extraHeaders != null)
            {
                foreach (var kvp in extraHeaders)
                    request.Headers[kvp.Key] = kvp.Value;
            }

            return request;
        }

        private static string ReadResponse(HttpWebRequest request)
        {
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string errorBody = reader.ReadToEnd();
                        throw new ApiException(
                            (int)((HttpWebResponse)ex.Response).StatusCode,
                            errorBody,
                            ex);
                    }
                }
                throw new ApiException(0, ex.Message, ex);
            }
        }

        /// <summary>
        /// Async GET — true async I/O via APM, does NOT block a thread pool thread.
        /// </summary>
        public static Task<string> GetAsync(string url, string apiKey, int timeoutMs = 15000, Dictionary<string, string> extraHeaders = null)
        {
            var request = CreateRequest(url, "GET", apiKey, timeoutMs, extraHeaders);
            return Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null)
                .ContinueWith(t => ReadResponseStream(t.Result));
        }

        /// <summary>
        /// Async POST — true async I/O via APM.
        /// </summary>
        public static Task<string> PostAsync(string url, string jsonBody, string apiKey, int timeoutMs = 60000, Dictionary<string, string> extraHeaders = null)
        {
            var request = CreateRequest(url, "POST", apiKey, timeoutMs, extraHeaders);
            request.ContentType = "application/json; charset=utf-8";
            byte[] bodyBytes = Encoding.UTF8.GetBytes(jsonBody ?? "");
            request.ContentLength = bodyBytes.Length;

            using (var stream = request.GetRequestStream())
                stream.Write(bodyBytes, 0, bodyBytes.Length);

            return Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null)
                .ContinueWith(t => ReadResponseStream(t.Result));
        }

        private static string ReadResponseStream(WebResponse response)
        {
            using (response)
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
                return reader.ReadToEnd();
        }
    }

    /// <summary>
    /// Exception for API errors with HTTP status code and response body.
    /// </summary>
    public class ApiException : Exception
    {
        public int StatusCode { get; }

        public ApiException(int statusCode, string message, Exception inner = null)
            : base(message, inner)
        {
            StatusCode = statusCode;
        }
    }
}
