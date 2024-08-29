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
            string file=Path.GetRandomFileName();
            PasswordRepository passwordRepository = new();
            PasswordModel password = new PasswordModel { Username = "admin", Password = "admin", Url = "admin.com" };
            passwordRepository.Add(password, "admin",file);
            Assert.NotNull(passwordRepository.GetAllPasswords("admin", file).FirstOrDefault(p => p.Id==password.Id));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases",file));
        }

        [Fact]
        public void ShouldRetrievePasswordById()
        {
            string file = Path.GetRandomFileName();
            PasswordRepository passwordRepository = new();
            PasswordModel password = new PasswordModel { Username = "admin", Password = "admin", Url = "admin.com" };
            passwordRepository.Add(password, "admin", file);
            Assert.NotNull(passwordRepository.GetPasswordById(password.Id,"admin",file));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file));
        }

        [Fact]
        public void ShouldEditPasswordWhenRequested()
        {
            string file = Path.GetRandomFileName();
            PasswordRepository passwordRepository = new();
            PasswordModel password = new PasswordModel { Username = "admin1", Password = "admin", Url = "admin.com" };
            PasswordModel passwordEdit = new PasswordModel { Username = "root", Password = "root", Url = "admin.com" };
            passwordRepository.Add(password, "admin", file);
            passwordRepository.Edit(password.Id, passwordEdit, "admin", file);
            Assert.Equal(passwordRepository.GetPasswordById(passwordEdit.Id,"admin",file),passwordEdit);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file));
        }

        [Fact]
        public void ShouldRemovePasswordWhenRequested()
        {
            string file = Path.GetRandomFileName();
            PasswordRepository passwordRepository = new();
            PasswordModel password = new PasswordModel { Username = "admin2", Password = "admin", Url = "admin.com" };
            passwordRepository.Add(password, "admin",file);
            passwordRepository.Remove(password.Id, "admin", file);
            Assert.Null(passwordRepository.GetAllPasswords("admin",file).FirstOrDefault(p => p==password));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file));
        }
    }
}
