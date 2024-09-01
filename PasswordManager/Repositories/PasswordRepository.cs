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
            passwords.Add(passwordModel);
            WritePasswords(passwords);
        }

        public void Edit(int id, PasswordModel newPasswordModel)
        {
            List<PasswordModel> passwords = GetAllPasswords();
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

            foreach (var password in passwords)
            {
                password.Password = "********";
            }

            return new(passwords.OrderBy(p => p.Url));
        }

        public PasswordModel GetPasswordById(int id)
        {
            return GetPasswordsFromFile().FirstOrDefault(p => p.Id == id);
        }

        public void Remove(int id)
        {
            List<PasswordModel> passwords = GetAllPasswords();
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
            UnicodeEncoding UE = new UnicodeEncoding();
            List<PasswordModel> passwords = new();
            using (var AES = Aes.Create())
            {
                byte[] passwordBytes = UE.GetBytes(Encoding.UTF8.GetString(ProtectedData.Unprotect(password, null, DataProtectionScope.CurrentUser)));
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

        private void WritePasswords(List<PasswordModel> passwords)
        {
            var fileName = Path.Combine(pathToDb, dataBaseName);
            UnicodeEncoding UE = new UnicodeEncoding();
            using (var AES = Aes.Create())
            {
                byte[] passwordBytes = UE.GetBytes(Encoding.UTF8.GetString(ProtectedData.Unprotect(password, null, DataProtectionScope.CurrentUser)));
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
