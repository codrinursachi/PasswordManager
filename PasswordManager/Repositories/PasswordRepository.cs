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
        readonly string fileName = Thread.CurrentPrincipal?.Identity?.Name + "EncryptedPass.db";

        public void Add(PasswordModel passwordModel, string encryptionData)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            ObservableCollection<PasswordModel> passwords = new();
            using (AesCryptoServiceProvider AES = new AesCryptoServiceProvider())
            {
                byte[] passwordBytes = UE.GetBytes(encryptionData);
                byte[] aesKey = SHA256.HashData(passwordBytes);
                byte[] aesIV = MD5.HashData(passwordBytes);
                AES.Key = aesKey;
                AES.IV = aesIV;
                ICryptoTransform encryptor = AES.CreateEncryptor();
                ICryptoTransform decryptor = AES.CreateDecryptor();
                using (FileStream fileStream = new(fileName, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (CryptoStream cryptoStream = new(fileStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new(cryptoStream))
                        {
                            if (File.ReadAllText(fileName).Length > 0)
                            {
                                string data = streamReader.ReadToEnd();
                                passwords = JsonSerializer.Deserialize<ObservableCollection<PasswordModel>>(data);
                            }
                        }
                    }
                }

                if (passwords.FirstOrDefault(p => p.Username == passwordModel.Username && p.Url == passwordModel.Url) != null)
                {
                    return;
                }

                passwords.Add(passwordModel);
                using (FileStream fileStream = new(fileName, FileMode.Open, FileAccess.Write))
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

        public void Edit(PasswordModel currentPasswordModel, PasswordModel newPasswordModel, string encryptionData)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<PasswordModel> GetAllPasswords(string encryptionData)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            ObservableCollection<PasswordModel> passwords = new();
            using (AesCryptoServiceProvider AES = new())
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
                                passwords = JsonSerializer.Deserialize<ObservableCollection<PasswordModel>>(data);
                            }
                        }
                    }
                }
            }

            return passwords;
        }

        public void Remove(PasswordModel passwordModel, string encryptionData)
        {
            throw new NotImplementedException();
        }
    }
}
