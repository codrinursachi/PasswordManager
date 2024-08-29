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
            PasswordModel password = new PasswordModel { Username = "admin", Password = "admin", Url = "admin.com" };
            passwordRepository.Add(password, "admin");
            Assert.NotNull(passwordRepository.GetAllPasswords("admin").FirstOrDefault(p => p.Id==password.Id));
        }

        [Fact]
        public void ShouldRetrievePasswordById()
        {
            PasswordRepository passwordRepository = new();
            PasswordModel password = new PasswordModel { Username = "admin", Password = "admin", Url = "admin.com" };
            passwordRepository.Add(password, "admin");
            Assert.NotNull(passwordRepository.GetPasswordById(password.Id,"admin"));
        }

        [Fact]
        public void ShouldEditPasswordWhenRequested()
        {
            PasswordRepository passwordRepository = new();
            PasswordModel password = new PasswordModel { Username = "admin1", Password = "admin", Url = "admin.com" };
            PasswordModel passwordEdit = new PasswordModel { Username = "root", Password = "root", Url = "admin.com" };
            passwordRepository.Add(password, "admin");
            passwordRepository.Edit(password.Id, passwordEdit,"admin");
            Assert.Equal(passwordRepository.GetPasswordById(passwordEdit.Id,"admin"),passwordEdit);
        }

        [Fact]
        public void ShouldRemovePasswordWhenRequested()
        {
            PasswordRepository passwordRepository = new();
            PasswordModel password = new PasswordModel { Username = "admin2", Password = "admin", Url = "admin.com" };
            passwordRepository.Add(password, "admin");
            passwordRepository.Remove(password.Id, "admin");
            Assert.Null(passwordRepository.GetAllPasswords("admin").FirstOrDefault(p => p==password));
        }
    }
}
