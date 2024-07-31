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
            PasswordModel password = new PasswordModel { Username = "admin", Password = "admin", Database = "default", Url = "admin.com" };
            passwordRepository.Add(password, "admin");
            Assert.NotNull(passwordRepository.GetAllPasswords("admin").FirstOrDefault(p => p.Username == "admin" && p.Password == "admin" && p.Database == "default" && p.Url == "admin.com"));
        }
    }
}
