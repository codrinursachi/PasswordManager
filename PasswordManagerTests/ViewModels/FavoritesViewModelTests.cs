using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.Services;
using PasswordManager.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManagerTests.ViewModels
{
    public class FavoritesViewModelTests
    {
        [Fact]
        public void ShouldStorePasswords()
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
            PasswordManagementService passwordManagementService = new(passwordRepository);
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com" };
            PasswordModel password2 = new() { Username = "admin2", Password = "admin2".ToCharArray(), Url = "admin2.com", Favorite = true };
            PasswordModel password3 = new() { Username = "admin3", Password = "admin3".ToCharArray(), Url = "admin3.com", Favorite = true };
            passwordRepository.Add(password);
            passwordRepository.Add(password2);
            passwordRepository.Add(password3);
            FavoritesViewModel allPasswordsViewModel = new(databaseInfoProviderService, passwordManagementService);
            allPasswordsViewModel.Refresh();
            Assert.Equal(2, allPasswordsViewModel.Passwords.Count);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file + ".json"));
        }

        [Fact]
        public void ShouldFilterPasswords()
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
            PasswordManagementService passwordManagementService = new(passwordRepository);
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com" };
            PasswordModel password2 = new() { Username = "admin2", Password = "admin2".ToCharArray(), Url = "admin2.com", Favorite = true };
            PasswordModel password3 = new() { Username = "admin3", Password = "admin3".ToCharArray(), Url = "admin3.com", Favorite = true };
            passwordRepository.Add(password);
            passwordRepository.Add(password2);
            passwordRepository.Add(password3);
            FavoritesViewModel allPasswordsViewModel = new(databaseInfoProviderService, passwordManagementService);
            allPasswordsViewModel.SearchFilter = "admin2";
            Assert.Single(allPasswordsViewModel.Passwords);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file + ".json"));
        }
    }
}
