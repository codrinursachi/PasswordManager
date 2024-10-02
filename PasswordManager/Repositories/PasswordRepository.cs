using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace PasswordManager.Repositories
{
    public class PasswordRepository
    {
        readonly string pathToDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases");
        readonly byte[] password;
        readonly string dataBaseName;

        public PasswordRepository(string dataBaseName, byte[] password)
        {
            this.password = password;
            this.dataBaseName = dataBaseName;
        }

        public void Add(PasswordModel passwordModel)
        {
            List<PasswordModel> passwords = GetPasswordsFromFile();
            passwordModel.Id = passwords.Count == 0 ? 1 : passwords.Max(p => p.Id) + 1;
            var encryptedPass = EncryptPass(passwordModel.Password);
            Array.Fill(passwordModel.Password, '0');
            passwordModel.Password = encryptedPass;
            passwords.Add(passwordModel);
            WritePasswords(passwords);
        }

        public void Edit(int id, PasswordModel newPasswordModel)
        {
            List<PasswordModel> passwords = GetPasswordsFromFile();
            var currentPassword = passwords.FirstOrDefault(p => p.Id == id);
            if (currentPassword == null)
            {
                return;
            }

            Remove(id);
            Add(newPasswordModel);
        }

        public List<PasswordModel> GetAllPasswords()
        {
            List<PasswordModel> passwords = GetPasswordsFromFile();

            return new(passwords.OrderBy(p => p.Url));
        }

        public PasswordModel GetPasswordById(int id)
        {
            var password = GetPasswordsFromFile().FirstOrDefault(p => p.Id == id);
            if (password == null)
            {
                return null;
            }

            password.Password=DecryptPass(password.Password);
            return password;
        }

        public void Remove(int id)
        {
            List<PasswordModel> passwords = GetPasswordsFromFile();
            passwords.Remove(passwords.First(p => p.Id == id));
            WritePasswords(passwords);
        }

        private List<PasswordModel> GetPasswordsFromFile()
        {
            if (dataBaseName == ".json")
            {
                return null;
            }
            var fileName = Path.Combine(pathToDb, dataBaseName);
            List<PasswordModel> passwords = new();
            using var AES = Aes.Create();

            byte[] passwordBytes = ProtectedData.Unprotect(password, null, DataProtectionScope.CurrentUser);
            byte[] aesKey = SHA256.HashData(passwordBytes);
            byte[] aesIV = MD5.HashData(passwordBytes);
            AES.Key = aesKey;
            AES.IV = aesIV;
            ICryptoTransform decryptor = AES.CreateDecryptor();
            using FileStream fileStream = new(fileName, FileMode.OpenOrCreate, FileAccess.Read);
            using CryptoStream cryptoStream = new(fileStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);
            string data = streamReader.ReadToEnd();
            if (data.Length > 0)
            {
                passwords = JsonSerializer.Deserialize<List<PasswordModel>>(data);
            }


            return passwords;
        }

        private void WritePasswords(List<PasswordModel> passwords)
        {
            var fileName = Path.Combine(pathToDb, dataBaseName);
            using var AES = Aes.Create();
            byte[] passwordBytes = ProtectedData.Unprotect(password, null, DataProtectionScope.CurrentUser);
            byte[] aesKey = SHA256.HashData(passwordBytes);
            byte[] aesIV = MD5.HashData(passwordBytes);
            AES.Key = aesKey;
            AES.IV = aesIV;
            ICryptoTransform encryptor = AES.CreateEncryptor();
            using FileStream fileStream = new(fileName, FileMode.Create, FileAccess.Write);
            using CryptoStream cryptoStream = new(fileStream, encryptor, CryptoStreamMode.Write);
            using StreamWriter streamWriter = new(cryptoStream);
            string data = JsonSerializer.Serialize(passwords);
            streamWriter.Write(data);
        }

        private char[] EncryptPass(char[] input)
        {
            using Aes AES = Aes.Create();
            byte[] passwordBytes = ProtectedData.Unprotect(password, null, DataProtectionScope.CurrentUser);
            byte[] aesKey = SHA256.HashData(passwordBytes);
            byte[] aesIV = MD5.HashData(passwordBytes);
            AES.Key = aesKey;
            AES.IV = aesIV;
            AES.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = AES.CreateEncryptor(AES.Key, AES.IV);

            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using StreamWriter srEncrypt = new StreamWriter(csEncrypt);
            srEncrypt.Write(input);
            srEncrypt.Flush();
            csEncrypt.FlushFinalBlock();
            var encrypted = msEncrypt.ToArray();

            return Convert.ToBase64String(encrypted).ToCharArray();
        }

        private char[] DecryptPass(char[] input)
        {
            byte[] inputBytes = Convert.FromBase64String(new string(input));
            using Aes AES = Aes.Create();
            byte[] passwordBytes = ProtectedData.Unprotect(password, null, DataProtectionScope.CurrentUser);
            byte[] aesKey = SHA256.HashData(passwordBytes);
            byte[] aesIV = MD5.HashData(passwordBytes);
            AES.Key = aesKey;
            AES.IV = aesIV;
            AES.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = AES.CreateDecryptor(AES.Key, AES.IV);

            using MemoryStream msDecrypt = new(inputBytes);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            return srDecrypt.ReadToEnd().ToCharArray();
        }
    }
}
