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
            Assert.NotNull(passwordRepository.GetAllPasswords("admin").FirstOrDefault(p => p.id==password.id));
        }

        [Fact]
        public void ShouldRetrievePasswordById()
        {
            PasswordRepository passwordRepository = new();
            PasswordModel password = new PasswordModel { Username = "admin", Password = "admin", Database = "default", Url = "admin.com" };
            passwordRepository.Add(password, "admin");
            Assert.NotNull(passwordRepository.GetPasswordById(password.id,"admin").FirstOrDefault(p => p.id == password.id));
        }

        [Fact]
        public void ShouldEditPasswordWhenRequested()
        {
            PasswordRepository passwordRepository = new();
            PasswordModel password = new PasswordModel { Username = "admin1", Password = "admin", Database = "default", Url = "admin.com" };
            PasswordModel passwordEdit = new PasswordModel { Username = "root", Password = "root", Database = "default", Url = "admin.com" };
            passwordRepository.Add(password, "admin");
            passwordRepository.Edit(password.id, passwordEdit,"admin");
            Assert.NotNull(passwordRepository.GetAllPasswords("admin").FirstOrDefault(p=>p==passwordEdit));
        }

        [Fact]
        public void ShouldRemovePasswordWhenRequested()
        {
            PasswordRepository passwordRepository = new();
            PasswordModel password = new PasswordModel { Username = "admin2", Password = "admin", Database = "default", Url = "admin.com" };
            passwordRepository.Add(password, "admin");
            passwordRepository.Remove(password.id, "admin");
            Assert.Null(passwordRepository.GetAllPasswords("admin").FirstOrDefault(p => p==password));
        }
    }
}
