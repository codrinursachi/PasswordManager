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
            PasswordRepository passwordRepository = new(file,"admin");
            PasswordModel password = new PasswordModel { Username = "admin", Password = "admin", Url = "admin.com" };
            passwordRepository.Add(password);
            Assert.NotNull(passwordRepository.GetAllPasswords().FirstOrDefault(p => p.Id==password.Id));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases",file));
        }

        [Fact]
        public void ShouldRetrievePasswordById()
        {
            string file = Path.GetRandomFileName();
            PasswordRepository passwordRepository = new(file, "admin");
            PasswordModel password = new PasswordModel { Username = "admin", Password = "admin", Url = "admin.com" };
            passwordRepository.Add(password);
            Assert.NotNull(passwordRepository.GetPasswordById(password.Id));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file));
        }

        [Fact]
        public void ShouldEditPasswordWhenRequested()
        {
            string file = Path.GetRandomFileName();
            PasswordRepository passwordRepository = new(file, "admin");
            PasswordModel password = new PasswordModel { Username = "admin1", Password = "admin", Url = "admin.com" };
            PasswordModel passwordEdit = new PasswordModel { Username = "root", Password = "root", Url = "admin.com" };
            passwordRepository.Add(password);
            passwordRepository.Edit(password.Id, passwordEdit);
            Assert.Equal(passwordRepository.GetPasswordById(passwordEdit.Id),passwordEdit);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file));
        }

        [Fact]
        public void ShouldRemovePasswordWhenRequested()
        {
            string file = Path.GetRandomFileName();
            PasswordRepository passwordRepository = new(file, "admin");
            PasswordModel password = new PasswordModel { Username = "admin2", Password = "admin", Url = "admin.com" };
            passwordRepository.Add(password);
            passwordRepository.Remove(password.Id);
            Assert.Null(passwordRepository.GetAllPasswords().FirstOrDefault(p => p==password));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file));
        }
    }
}
