using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.Services;
using PasswordManager.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests.Repositories
{
    public class PasswordRepositoryTests
    {
        [Fact]
        public void ShouldAddAndRetrievePasswordsCorrectly()
        {
            string file = Path.Combine("Temporary", Path.GetRandomFileName());
            string pathToTemp= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", "Temporary");
            if (!Directory.Exists(pathToTemp))
            {
                Directory.CreateDirectory(pathToTemp);
            }
            DatabaseInfoProviderService databaseInfoProviderService = new()
            {
                CurrentDatabase = file,
                DBPass = ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser)
            };
            PasswordRepository passwordRepository = new(databaseInfoProviderService);
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com" };
            passwordRepository.Add(password);
            Assert.NotNull(passwordRepository.GetAllPasswords().FirstOrDefault(p => p.Id == password.Id));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file + ".json"));
        }

        [Fact]
        public void ShouldRetrievePasswordById()
        {
            string file = Path.Combine("Temporary", Path.GetRandomFileName());
            string pathToTemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", "Temporary");
            if (!Directory.Exists(pathToTemp))
            {
                Directory.CreateDirectory(pathToTemp);
            }
            DatabaseInfoProviderService databaseInfoProviderService = new()
            {
                CurrentDatabase = file,
                DBPass = ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser)
            };
            PasswordRepository passwordRepository = new(databaseInfoProviderService);
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com" };
            passwordRepository.Add(password);
            Assert.NotNull(passwordRepository.GetPasswordById(password.Id));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file + ".json"));
        }

        [Fact]
        public void ShouldEditPasswordWhenRequested()
        {
            string file = Path.Combine("Temporary", Path.GetRandomFileName());
            string pathToTemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", "Temporary");
            if (!Directory.Exists(pathToTemp))
            {
                Directory.CreateDirectory(pathToTemp);
            }
            DatabaseInfoProviderService databaseInfoProviderService = new()
            {
                CurrentDatabase = file,
                DBPass = ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser)
            };
            PasswordRepository passwordRepository = new(databaseInfoProviderService);
            PasswordModel password = new() { Username = "admin1", Password = "admin".ToCharArray(), Url = "admin.com" };
            PasswordModel passwordEdit = new() { Username = "root", Password = "root".ToCharArray(), Url = "admin.com" };
            passwordRepository.Add(password);
            passwordRepository.Edit(password.Id, passwordEdit);
            Assert.Equal(passwordRepository.GetPasswordById(password.Id).Username, passwordEdit.Username);
            Assert.Equal(passwordRepository.GetPasswordById(password.Id).Password, passwordEdit.Password);
            Assert.Equal(passwordRepository.GetPasswordById(password.Id).Url, passwordEdit.Url);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file + ".json"));
        }

        [Fact]
        public void ShouldRemovePasswordWhenRequested()
        {
            string file = Path.Combine("Temporary", Path.GetRandomFileName());
            string pathToTemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", "Temporary");
            if (!Directory.Exists(pathToTemp))
            {
                Directory.CreateDirectory(pathToTemp);
            }
            DatabaseInfoProviderService databaseInfoProviderService = new()
            {
                CurrentDatabase = file,
                DBPass = ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser)
            };
            PasswordRepository passwordRepository = new(databaseInfoProviderService);
            PasswordModel password = new() { Username = "admin2", Password = "admin".ToCharArray(), Url = "admin.com" };
            passwordRepository.Add(password);
            passwordRepository.Remove(password.Id);
            Assert.Null(passwordRepository.GetAllPasswords().FirstOrDefault(p => p == password));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file + ".json"));
        }
    }
}
