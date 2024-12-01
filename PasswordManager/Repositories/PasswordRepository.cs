﻿using PasswordManager.Interfaces;
using PasswordManager.Models;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PasswordManager.Repositories
{
    public class PasswordRepository : IPasswordRepository
    {
        readonly string pathToDb;
        private IDatabaseInfoProviderService databaseInfoProviderService;
        public PasswordRepository(
            IDatabaseInfoProviderService databaseInfoProviderService,
            IPathProviderService pathProviderService)
        {
            this.databaseInfoProviderService = databaseInfoProviderService;
            pathToDb = Path.Combine(pathProviderService.ProgramPath, "Databases");
        }

        public void Add(PasswordModel passwordModel)
        {
            var dbPath = Path.Combine(pathToDb, databaseInfoProviderService.CurrentDatabase + ".json");
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

            return new(passwords?.OrderBy(p => p.Url).ToList() ?? []);
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
            var fileName = Path.Combine(pathToDb, databaseInfoProviderService.CurrentDatabase + ".json");
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
            var fileName = Path.Combine(pathToDb, databaseInfoProviderService.CurrentDatabase + ".json");
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
