using System;
using System.Collections.Generic;
using DeepJeb.Protocol.Anthropic;
using DeepJeb.Protocol.Google;
using DeepJeb.Protocol.OpenAI;

namespace DeepJeb.Protocol
{
    /// <summary>
    /// Creates the correct API client implementation based on ProviderConfig.
    ///
    /// The factory maps ProtocolType → concrete client class:
    ///   OpenAI   → OpenAiClient   (also serves 11+ OpenAI-compatible providers)
    ///   Anthropic → AnthropicClient
    ///   Google   → GoogleClient
    /// </summary>
    public class ApiClientFactory
    {
        /// <summary>
        /// Create an API client from a provider configuration.
        /// </summary>
        public static object CreateClient(ProviderConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            switch (config.Protocol)
            {
                case ProtocolType.OpenAI:
                    return new OpenAiClient(config.Name, config.ApiKey, config.BaseUrl);

                case ProtocolType.Anthropic:
                    return new AnthropicClient(config.Name, config.ApiKey, config.BaseUrl);

                case ProtocolType.Google:
                    return new GoogleClient(config.Name, config.ApiKey, config.BaseUrl,
                        config.GoogleQueryParamAuth);

                default:
                    throw new ArgumentException($"Unknown protocol type: {config.Protocol}");
            }
        }

        /// <summary>
        /// Create an OpenAI-compatible client (convenience overload).
        /// </summary>
        public static OpenAiClient CreateOpenAi(string providerName, string apiKey, string baseUrl)
        {
            return new OpenAiClient(providerName, apiKey, baseUrl);
        }

        /// <summary>
        /// Create an Anthropic client (convenience overload).
        /// </summary>
        public static AnthropicClient CreateAnthropic(string providerName, string apiKey, string baseUrl)
        {
            return new AnthropicClient(providerName, apiKey, baseUrl);
        }

        /// <summary>
        /// Create a Google Gemini client (convenience overload).
        /// </summary>
        public static GoogleClient CreateGoogle(string providerName, string apiKey, string baseUrl,
            bool queryParamAuth = true)
        {
            return new GoogleClient(providerName, apiKey, baseUrl, queryParamAuth);
        }

        /// <summary>
        /// Build a list of typed client wrappers from configurations.
        /// Returns a list that can be queried for provider name, model switching, etc.
        /// </summary>
        public static List<ApiClientEntry> CreateAll(IEnumerable<ProviderConfig> configs)
        {
            var entries = new List<ApiClientEntry>();
            foreach (var config in configs)
            {
                var client = CreateClient(config);
                entries.Add(new ApiClientEntry
                {
                    Config = config,
                    Client = client,
                    ClientType = client.GetType()
                });
            }
            return entries;
        }
    }

    /// <summary>
    /// Associates a ProviderConfig with its instantiated client object.
    /// Used by the UI to display provider/model menus.
    /// </summary>
    public class ApiClientEntry
    {
        public ProviderConfig Config { get; set; }
        public object Client { get; set; }
        public Type ClientType { get; set; }

        public IOpenAiCompatibleApi AsOpenAi() => Client as IOpenAiCompatibleApi;
        public IAnthropicCompatibleApi AsAnthropic() => Client as IAnthropicCompatibleApi;
        public IGoogleGenerativeApi AsGoogle() => Client as IGoogleGenerativeApi;

        public string ProviderName => Config?.Name ?? "Unknown";
        public ProtocolType Protocol => Config?.Protocol ?? ProtocolType.OpenAI;
    }
}
