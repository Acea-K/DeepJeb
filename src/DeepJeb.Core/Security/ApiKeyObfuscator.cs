using System;
using System.Text;

namespace DeepJeb.Core.Security
{
    /// <summary>
    /// XOR + Base64 obfuscation for API keys stored on disk.
    ///
    /// This is NOT encryption — it prevents casual snooping (e.g. streaming
    /// the config file or glancing at it). A determined attacker with disk
    /// access can reverse it trivially. The threat model is "hide keys from
    /// casual observation," not "defeat forensic analysis."
    ///
    /// Obfuscate: plaintext → XOR with key → Base64
    /// Deobfuscate: Base64 → XOR with key → plaintext
    /// </summary>
    public static class ApiKeyObfuscator
    {
        // Fixed XOR key. Combined with a per-machine salt for basic diversity.
        private static readonly byte[] XorKey = Encoding.UTF8.GetBytes(
            "DeepJeb_KSP_AI_Assistant_2026_SecureKey_XOR_Pad");

        /// <summary>
        /// Encode a plaintext API key for disk storage.
        /// Returns Base64-encoded XORed bytes, or null if input is null/empty.
        /// </summary>
        public static string Obfuscate(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
                return plaintext;

            byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] obfuscated = new byte[plainBytes.Length];

            for (int i = 0; i < plainBytes.Length; i++)
                obfuscated[i] = (byte)(plainBytes[i] ^ XorKey[i % XorKey.Length]);

            return Convert.ToBase64String(obfuscated);
        }

        /// <summary>
        /// Decode an obfuscated API key from disk storage.
        /// Returns the plaintext string, or null if input is null/empty.
        /// If the input is not valid Base64, returns the input as-is
        /// (graceful fallback for plaintext keys from before obfuscation).
        /// </summary>
        public static string Deobfuscate(string obfuscated)
        {
            if (string.IsNullOrEmpty(obfuscated))
                return obfuscated;

            byte[] obfuscatedBytes;
            try
            {
                obfuscatedBytes = Convert.FromBase64String(obfuscated);
            }
            catch (FormatException)
            {
                // Not valid Base64 — likely an old plaintext key. Return as-is.
                return obfuscated;
            }

            byte[] plainBytes = new byte[obfuscatedBytes.Length];

            for (int i = 0; i < obfuscatedBytes.Length; i++)
                plainBytes[i] = (byte)(obfuscatedBytes[i] ^ XorKey[i % XorKey.Length]);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
