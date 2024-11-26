using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.Services;
using PasswordManager.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManagerTests.ViewModels
{
    public class CategoryViewModelTests
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
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com", CategoryPath = "admin\\1" };
            PasswordModel password2 = new() { Username = "admin2", Password = "admin2".ToCharArray(), Url = "admin2.com", CategoryPath = "admin\\2" };
            PasswordModel password3 = new() { Username = "admin3", Password = "admin3".ToCharArray(), Url = "admin3.com", CategoryPath = "admin\\3" };
            passwordRepository.Add(password);
            passwordRepository.Add(password2);
            passwordRepository.Add(password3);
            CategoryViewModel categoryViewModel = new(databaseInfoProviderService, passwordManagementService);
            categoryViewModel.Refresh();
            Assert.Equal(3, categoryViewModel.Passwords.Count);
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
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com", CategoryPath = "admin\\1" };
            PasswordModel password2 = new() { Username = "admin2", Password = "admin2".ToCharArray(), Url = "admin2.com", CategoryPath = "admin\\2" };
            PasswordModel password3 = new() { Username = "admin3", Password = "admin3".ToCharArray(), Url = "admin3.com", CategoryPath = "admin\\3" };
            passwordRepository.Add(password);
            passwordRepository.Add(password2);
            passwordRepository.Add(password3);
            CategoryViewModel categoryViewModel = new(databaseInfoProviderService, passwordManagementService);
            var root = new CategoryNodeModel { Name = "Categories" };
            var parts = "admin\\2".Split('\\');
            var current = root;
            foreach (var part in parts)
            {
                var child = current.Children.FirstOrDefault(p => p.Name == part);
                if (child == null)
                {
                    child = new CategoryNodeModel { Name = part, Parent = current };
                    current.Children.Add(child);
                }

                current = child;
            }
            categoryViewModel.Filter = current;
            Assert.Single(categoryViewModel.Passwords);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file + ".json"));
        }
    }
}
