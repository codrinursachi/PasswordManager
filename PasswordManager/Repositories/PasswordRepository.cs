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
        readonly string fileName = ".\\"+Thread.CurrentPrincipal.Identity.Name+"\\"+App.Current.Properties["SelectedDb"]?.ToString();

        public void Add(PasswordModel passwordModel, string encryptionData)
        {
            List<PasswordModel> passwords = GetPasswordsFromFile(encryptionData);
            passwordModel.Id = passwords.Count == 0 ? 1 : passwords.Max(p => p.Id) + 1;
            passwords.Add(passwordModel);
            WritePasswords(encryptionData, passwords);
        }

        public void Edit(int id, PasswordModel newPasswordModel, string encryptionData)
        {
            List<PasswordModel> passwords = GetAllPasswords(encryptionData);
            var currentPassword = passwords.FirstOrDefault(p => p.Id == id);
            if (currentPassword == null)
            {
                return;
            }

            Remove(id, encryptionData);
            Add(newPasswordModel, encryptionData);
        }

        public List<PasswordModel> GetAllPasswords(string encryptionData)
        {
            List<PasswordModel> passwords = GetPasswordsFromFile(encryptionData);

            foreach (var password in passwords)
            {
                password.Password = "********";
            }

            return new(passwords.OrderBy(p => p.Url));
        }

        public PasswordModel GetPasswordById(int id, string encryptionData)
        {
            return GetPasswordsFromFile(encryptionData).FirstOrDefault(p => p.Id == id);
        }

        public void Remove(int id, string encryptionData)
        {
            List<PasswordModel> passwords = GetAllPasswords(encryptionData);
            passwords.Remove(passwords.First(p => p.Id == id));
            WritePasswords(encryptionData, passwords);
        }

        private List<PasswordModel> GetPasswordsFromFile(string encryptionData)
        {
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

        private void WritePasswords(string encryptionData, List<PasswordModel> passwords)
        {
            using (var AES = Aes.Create())
            {
                UnicodeEncoding UE = new UnicodeEncoding();
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
