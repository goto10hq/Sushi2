using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sushi2
{
    public static class HashTools
    {
        /// <summary>
        /// Hash types.
        /// </summary>
        public enum HashType
        {
            /// <summary>
            /// MD5 hashing.
            /// </summary>
            [Obsolete("Do not use insecure cryptographic algorithm MD5.")]
            MD5,
            /// <summary>
            /// SHA1 hashing.
            /// </summary>
            [Obsolete("Do not use insecure cryptographic algorithm SHA1.")]
            SHA1,
            /// <summary>
            /// SHA256 hashing.
            /// </summary>
            SHA256,
            /// <summary>
            /// SHA384 hashing.
            /// </summary>
            SHA384,
            /// <summary>
            /// SHA512 hashing.
            /// </summary>
            SHA512
        }

        static readonly Dictionary<HashType, Func<string, Encoding, string>> _algorithms = new Dictionary<HashType, Func<string, Encoding, string>>
        {
            { HashType.MD5, GetMD5 },
            { HashType.SHA1, GetSHA1 },
            { HashType.SHA256, GetSHA256 },
            { HashType.SHA384, GetSHA384 },
            { HashType.SHA512, GetSHA512 }
        };

        /// <summary>
        /// Get a hash string acoording to the given hash algorithm and text.
        /// </summary>
        /// <param name="hashType">Hash algorithm.</param>
        /// <param name="text">String to hash.</param>
        /// <param name="encoding">Encoding.</param>
        /// <returns>Hashed string (HEX form).</returns>
        public static string GetHash(string text, HashType hashType = HashType.SHA256, Encoding encoding = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (!_algorithms.ContainsKey(hashType))
                throw new ArgumentOutOfRangeException(nameof(hashType));

            if (encoding == null)
                encoding = Encoding.Unicode;

            return _algorithms[hashType].Invoke(text, encoding);
        }

        static string GetMD5(string text, Encoding encoding)
        {
            var messageBytes = encoding.GetBytes(text);
#pragma warning disable CA5351 // Do not use insecure cryptographic algorithm MD5.
            var md5 = MD5.Create();
#pragma warning restore CA5351 // Do not use insecure cryptographic algorithm MD5.
            var hashValue = md5.ComputeHash(messageBytes);
            return hashValue.Aggregate("", (current, t) => current + string.Format(Cultures.Invariant, "{0:x2}", t));
        }

        static string GetSHA1(string text, Encoding encoding)
        {
            var messageBytes = encoding.GetBytes(text);
#pragma warning disable CA5350 // Do not use insecure cryptographic algorithm SHA1.
            var md5 = SHA1.Create();
#pragma warning restore CA5350 // Do not use insecure cryptographic algorithm SHA1.
            var hashValue = md5.ComputeHash(messageBytes);
            return hashValue.Aggregate("", (current, t) => current + string.Format(Cultures.Invariant, "{0:x2}", t));
        }

        static string GetSHA256(string text, Encoding encoding)
        {
            var messageBytes = encoding.GetBytes(text);
            var md5 = SHA256.Create();
            var hashValue = md5.ComputeHash(messageBytes);
            return hashValue.Aggregate("", (current, t) => current + string.Format(Cultures.Invariant, "{0:x2}", t));
        }

        static string GetSHA384(string text, Encoding encoding)
        {
            var messageBytes = encoding.GetBytes(text);
            var md5 = SHA384.Create();
            var hashValue = md5.ComputeHash(messageBytes);
            return hashValue.Aggregate("", (current, t) => current + string.Format(Cultures.Invariant, "{0:x2}", t));
        }

        static string GetSHA512(string text, Encoding encoding)
        {
            var messageBytes = encoding.GetBytes(text);
            var md5 = SHA512.Create();
            var hashValue = md5.ComputeHash(messageBytes);
            return hashValue.Aggregate("", (current, t) => current + string.Format(Cultures.Invariant, "{0:x2}", t));
        }
    }
}