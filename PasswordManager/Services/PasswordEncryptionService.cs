using PasswordManager.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Services
{
    public class PasswordEncryptionService : IPasswordEncryptionService
    {
        private IDatabaseInfoProviderService databaseInfoProviderService;

        public PasswordEncryptionService(
            IDatabaseInfoProviderService databaseInfoProviderService)
        {
            this.databaseInfoProviderService = databaseInfoProviderService;
        }

        public char[] Decrypt(char[] input)
        {
            byte[] inputBytes = Convert.FromBase64String(new string(input));
            using Aes AES = GenerateAES();
            return Encoding.UTF8.GetChars(AES.DecryptCbc(inputBytes, AES.IV));
        }

        public char[] Encrypt(char[] input)
        {
            using Aes AES = GenerateAES();
            return Convert.ToBase64String(AES.EncryptCbc(Encoding.UTF8.GetBytes(input), AES.IV)).ToCharArray();
        }

        private Aes GenerateAES()
        {
            Aes AES = Aes.Create();
            byte[] passwordBytes = ProtectedData.Unprotect(databaseInfoProviderService.DBPass, null, DataProtectionScope.CurrentUser);
            byte[] aesKey = SHA256.HashData(passwordBytes);
            byte[] aesIV = MD5.HashData(passwordBytes);
            AES.Key = aesKey;
            AES.IV = aesIV;
            AES.Padding = PaddingMode.PKCS7;
            Array.Clear(passwordBytes);
            return AES;
        }
    }
}
