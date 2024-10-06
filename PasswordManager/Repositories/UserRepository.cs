using PasswordManager.Models;
using PasswordManager.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PasswordManager.Repositories
{
    public class UserRepository
    {
        readonly string fileName;

        public UserRepository(string loginFileName)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            fileName = Path.Combine(path, loginFileName);
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }
        }

        public void Add(char[] password)
        {
            var passwordHash = SecretHasher.Hash(password, 50000);
            File.WriteAllText(fileName, passwordHash);
        }

        public bool AuthenticateUser(char[] password)
        {
            string passwordHash = File.ReadAllText(fileName);
            if (string.IsNullOrEmpty(passwordHash))
            {
                Add(password);
                return true;
            }

            return SecretHasher.Verify(password, passwordHash);
        }
    }
}
