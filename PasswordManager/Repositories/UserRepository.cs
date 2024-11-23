using PasswordManager.Interfaces;
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
    public class UserRepository:IUserRepository
    {
        readonly string fileName;

        public UserRepository(
            IPathProviderService pathProviderService)
        {
            fileName = Path.Combine(pathProviderService.ProgramPath, "UserLogin");
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }
        }

        private void Add(char[] password)
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
