using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests
{
    public class PasswordRepositoryTests
    {
        [Fact]
        public void ShouldAddAndRetrievePasswordsCorrectly()
        {
            PasswordRepository passwordRepository = new();
            PasswordModel password = new PasswordModel { username = "admin", password = "admin", database = "default", url = "admin.com" };
            passwordRepository.Add(password, SecretHasher.Hash("admin", 40000));
            Assert.NotNull(passwordRepository.GetAllPasswords(SecretHasher.Hash("admin", 40000)).FirstOrDefault(p => p.username == "admin" && p.password == "admin" && p.database == "default" && p.url == "admin.com"));
        }
    }
}
