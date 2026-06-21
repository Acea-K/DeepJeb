using System.Collections.Generic;

namespace DeepJeb.Protocol
{
    /// <summary>
    /// Identifies which protocol family an API provider uses.
    /// </summary>
    public enum ProtocolType
    {
        OpenAI,
        Anthropic,
        Google
    }

    /// <summary>
    /// Configuration for one API provider.
    /// Stored in DeepJeb.cfg as JSON.
    ///
    /// Field names are lowercase for clean JSON output via JsonMapper.
    /// </summary>
    public class ProviderConfig
    {
        /// <summary>Display name (e.g. "OpenAI", "DeepSeek").</summary>
        public string Name { get; set; }

        /// <summary>Which protocol the provider speaks.</summary>
        public ProtocolType Protocol { get; set; }

        /// <summary>API key (stored XOR+Base64 on disk, plaintext in memory).</summary>
        public string ApiKey { get; set; }

        /// <summary>Base URL for the API endpoint.</summary>
        public string BaseUrl { get; set; }

        /// <summary>Models enabled for this provider.</summary>
        public List<string> EnabledModels { get; set; }

        /// <summary>Google-specific: API key as query parameter.</summary>
        public bool GoogleQueryParamAuth { get; set; } = true;

        /// <summary>Default reasoning effort (o1/o3 series).</summary>
        public string ReasoningEffort { get; set; }

        /// <summary>Thinking mode enabled (Anthropic).</summary>
        public bool ThinkingEnabled { get; set; }

        /// <summary>Thinking budget in tokens.</summary>
        public int ThinkingBudgetTokens { get; set; } = 16000;

        public ProviderConfig()
        {
            EnabledModels = new List<string>();
        }

        /// <summary>
        /// Built-in provider presets.
        /// </summary>
        public static List<ProviderConfig> BuiltInProviders()
        {
            return new List<ProviderConfig>
            {
                new ProviderConfig { Name = "OpenAI",         Protocol = ProtocolType.OpenAI,   BaseUrl = "https://api.openai.com" },
                new ProviderConfig { Name = "Anthropic",      Protocol = ProtocolType.Anthropic, BaseUrl = "https://api.anthropic.com" },
                new ProviderConfig { Name = "Google Gemini",  Protocol = ProtocolType.Google,   BaseUrl = "https://generativelanguage.googleapis.com" },
                new ProviderConfig { Name = "DeepSeek",       Protocol = ProtocolType.OpenAI,   BaseUrl = "https://api.deepseek.com" },
                new ProviderConfig { Name = "OpenRouter",     Protocol = ProtocolType.OpenAI,   BaseUrl = "https://openrouter.ai/api" },
                new ProviderConfig { Name = "Grok (xAI)",     Protocol = ProtocolType.OpenAI,   BaseUrl = "https://api.x.ai" },
                new ProviderConfig { Name = "Mistral AI",     Protocol = ProtocolType.OpenAI,   BaseUrl = "https://api.mistral.ai" },
                new ProviderConfig { Name = "Moonshot",       Protocol = ProtocolType.OpenAI,   BaseUrl = "https://api.moonshot.cn" },
                new ProviderConfig { Name = "MiniMax",        Protocol = ProtocolType.OpenAI,   BaseUrl = "https://api.minimax.chat" },
                new ProviderConfig { Name = "Qwen (Alibaba)", Protocol = ProtocolType.OpenAI,   BaseUrl = "https://dashscope.aliyuncs.com/compatible-mode" },
                new ProviderConfig { Name = "Xiaomi MiMo",    Protocol = ProtocolType.OpenAI,   BaseUrl = "https://api.xiaomi.com" },
                new ProviderConfig { Name = "Z.ai",           Protocol = ProtocolType.OpenAI,   BaseUrl = "https://api.z.ai" },
            };
        }
    }
}
