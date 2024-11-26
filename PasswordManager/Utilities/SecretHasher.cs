using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Utilities
{
    public static class SecretHasher
    {
        private const int saltSize = 16; // 128 bits
        private const int keySize = 32; // 256 bits
        private static readonly HashAlgorithmName algorithm = HashAlgorithmName.SHA256;

        private const char segmentDelimiter = ':';

        public static string Hash(char[] input, int iterations)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            byte[] pass = Encoding.UTF8.GetBytes(input);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                pass,
                salt,
                iterations,
                algorithm,
                keySize
            );
            return string.Join(
                segmentDelimiter,
                Convert.ToHexString(hash),
                Convert.ToHexString(salt),
                iterations,
                algorithm
            );
        }

        public static bool Verify(char[] input, string hashString)
        {
            string[] segments = hashString.Split(segmentDelimiter);
            byte[] hash = Convert.FromHexString(segments[0]);
            byte[] salt = Convert.FromHexString(segments[1]);
            byte[] pass = Encoding.UTF8.GetBytes(input);
            int iterations = int.Parse(segments[2]);
            HashAlgorithmName algorithm = new HashAlgorithmName(segments[3]);
            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                pass,
                salt,
                iterations,
                algorithm,
                hash.Length
            );
            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }
    }
}
