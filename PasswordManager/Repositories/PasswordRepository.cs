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
using System.Windows.Media;

namespace PasswordManager.Repositories
{
    public class PasswordRepository : IPasswordRepository
    {
        readonly string pathToDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases");

        public void Add(PasswordModel passwordModel, string encryptionData, string dataBaseName)
        {
            List<PasswordModel> passwords = GetPasswordsFromFile(encryptionData,dataBaseName);
            passwordModel.Id = passwords.Count == 0 ? 1 : passwords.Max(p => p.Id) + 1;
            passwords.Add(passwordModel);
            WritePasswords(encryptionData, passwords, dataBaseName);
        }

        public void Edit(int id, PasswordModel newPasswordModel, string encryptionData, string dataBaseName)
        {
            List<PasswordModel> passwords = GetAllPasswords(encryptionData,dataBaseName);
            var currentPassword = passwords.FirstOrDefault(p => p.Id == id);
            if (currentPassword == null)
            {
                return;
            }

            Remove(id, encryptionData, dataBaseName);
            Add(newPasswordModel, encryptionData, dataBaseName);
        }

        public List<PasswordModel> GetAllPasswords(string encryptionData, string dataBaseName)
        {
            List<PasswordModel> passwords = GetPasswordsFromFile(encryptionData,dataBaseName);

            foreach (var password in passwords)
            {
                password.Password = "********";
            }

            return new(passwords.OrderBy(p => p.Url));
        }

        public PasswordModel GetPasswordById(int id, string encryptionData, string dataBaseName)
        {
            return GetPasswordsFromFile(encryptionData, dataBaseName).FirstOrDefault(p => p.Id == id);
        }

        public void Remove(int id, string encryptionData, string dataBaseName)
        {
            List<PasswordModel> passwords = GetAllPasswords(encryptionData, dataBaseName);
            passwords.Remove(passwords.First(p => p.Id == id));
            WritePasswords(encryptionData, passwords, dataBaseName);
        }

        private List<PasswordModel> GetPasswordsFromFile(string encryptionData, string dataBaseName)
        {
            if (dataBaseName==".json")
            {
                return new();
            }
            var fileName = Path.Combine(pathToDb, dataBaseName);
            UnicodeEncoding UE = new UnicodeEncoding();
            List<PasswordModel> passwords = new();
            using (var AES = Aes.Create())
            {
                byte[] passwordBytes = UE.GetBytes(encryptionData);
                byte[] aesKey = SHA256.HashData(passwordBytes);
                byte[] aesIV = MD5.HashData(passwordBytes);
                AES.Key = aesKey;
                AES.IV = aesIV;
                ICryptoTransform decryptor = AES.CreateDecryptor();
                using (FileStream fileStream = new(fileName, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (CryptoStream cryptoStream = new(fileStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new(cryptoStream))
                        {
                            string data = streamReader.ReadToEnd();
                            if (data.Length > 0)
                            {
                                passwords = JsonSerializer.Deserialize<List<PasswordModel>>(data);
                            }
                        }
                    }
                }
            }

            return passwords;
        }

        private void WritePasswords(string encryptionData, List<PasswordModel> passwords, string dataBaseName)
        {
            var fileName = Path.Combine(pathToDb, dataBaseName);
            UnicodeEncoding UE = new UnicodeEncoding();
            using (var AES = Aes.Create())
            {
                byte[] passwordBytes = UE.GetBytes(encryptionData);
                byte[] aesKey = SHA256.HashData(passwordBytes);
                byte[] aesIV = MD5.HashData(passwordBytes);
                AES.Key = aesKey;
                AES.IV = aesIV;
                ICryptoTransform encryptor = AES.CreateEncryptor();
                using (FileStream fileStream = new(fileName, FileMode.Create, FileAccess.Write))
                {
                    using (CryptoStream cryptoStream = new(fileStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new(cryptoStream))
                        {
                            string data = JsonSerializer.Serialize(passwords);
                            streamWriter.Write(data);
                        }
                    }
                }
            }
        }
    }
}
