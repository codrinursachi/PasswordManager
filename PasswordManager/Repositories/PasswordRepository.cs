using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
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
            this.dataBaseName = dataBaseName + ".json";
            var dbPath = Path.Combine(pathToDb, this.dataBaseName);
            if (!File.Exists(dbPath))
            {
                File.Create(dbPath).Close();
            }
        }

        public void Add(PasswordModel passwordModel)
        {
            List<PasswordModel> passwords = GetPasswordsFromFile();
            passwordModel.Id = passwords.Count == 0 ? 1 : passwords.Max(p => p.Id) + 1;
            var encryptedPass = Encrypt(passwordModel.Password);
            Array.Fill(passwordModel.Password, '0');
            passwordModel.Password = encryptedPass.ToCharArray();
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

            currentPassword.Url = newPasswordModel.Url;
            currentPassword.Username = newPasswordModel.Username;
            currentPassword.Password = Encrypt(newPasswordModel.Password).ToCharArray();
            currentPassword.ExpirationDate = newPasswordModel.ExpirationDate;
            currentPassword.CategoryPath = newPasswordModel.CategoryPath;
            currentPassword.Favorite = newPasswordModel.Favorite;
            currentPassword.Notes = newPasswordModel.Notes;
            currentPassword.Tags = newPasswordModel.Tags;
            WritePasswords(passwords);
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

            password.Password = Decrypt(password.Password).ToCharArray();
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
            List<PasswordModel> passwords = [];

            string data = File.ReadAllText(fileName);
            if (data.Length > 0)
            {
                passwords = JsonSerializer.Deserialize<List<PasswordModel>>(Decrypt(data.ToCharArray()));
            }


            return passwords;
        }

        private void WritePasswords(List<PasswordModel> passwords)
        {
            var fileName = Path.Combine(pathToDb, dataBaseName);
            File.WriteAllText(fileName, Encrypt(JsonSerializer.Serialize(passwords).ToCharArray()));
        }

        private string Encrypt(char[] input)
        {
            using Aes AES = GenerateAES();
            return Convert.ToBase64String(AES.EncryptCbc(Encoding.UTF8.GetBytes(input), AES.IV));
        }

        private string Decrypt(char[] input)
        {
            byte[] inputBytes = Convert.FromBase64String(new string(input));
            using Aes AES = GenerateAES();
            return Encoding.UTF8.GetString(AES.DecryptCbc(inputBytes, AES.IV));
        }

        private Aes GenerateAES()
        {
            Aes AES = Aes.Create();
            byte[] passwordBytes = ProtectedData.Unprotect(password, null, DataProtectionScope.CurrentUser);
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
