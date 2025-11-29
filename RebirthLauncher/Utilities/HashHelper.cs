// RebirthLauncher/Utilities/HashHelper.cs
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RebirthLauncher.Utilities
{
    /// <summary>
    /// Small centralized helper for MD5 hashing used by the bootstrapper.
    /// Keeps hashing logic in one place so the algorithm can be swapped later if needed.
    /// </summary>
    internal static class HashHelper
    {
        /// <summary>
        /// Compute the MD5 hash of the provided byte array and return a lowercase hex string.
        /// </summary>
        public static string ComputeMd5Hex(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(data);
                return ToHexLower(hash);
            }
        }

        /// <summary>
        /// Compute the MD5 hash of the provided stream and return a lowercase hex string.
        /// The stream position will be reset to its original value if the stream supports seeking.
        /// </summary>
        public static string ComputeMd5Hex(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            long originalPosition = 0;
            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                using (var md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(stream);
                    return ToHexLower(hash);
                }
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        /// <summary>
        /// Compute the MD5 hash of a file on disk and return a lowercase hex string.
        /// </summary>
        public static string ComputeMd5HexFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            using (var fs = File.OpenRead(filePath))
            {
                return ComputeMd5Hex(fs);
            }
        }

        private static string ToHexLower(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}