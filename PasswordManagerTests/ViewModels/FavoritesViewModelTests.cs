using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.ViewModels;
using PasswordManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PasswordManager.Views;
using static System.Net.Mime.MediaTypeNames;

namespace PasswordManagerTests.ViewModels
{
    public class FavoritesViewModelTests
    {
        [Fact]
        public void ShouldStorePasswords()
        {
            string file = Path.GetRandomFileName();
            PasswordRepository passwordRepository = new(file, "admin");
            PasswordModel password = new PasswordModel { Username = "admin", Password = "admin", Url = "admin.com" };
            PasswordModel password2 = new PasswordModel { Username = "admin2", Password = "admin2", Url = "admin2.com", Favorite = true };
            PasswordModel password3 = new PasswordModel { Username = "admin3", Password = "admin3", Url = "admin3.com", Favorite = true };
            passwordRepository.Add(password);
            passwordRepository.Add(password2);
            passwordRepository.Add(password3);
            FavoritesViewModel allPasswordsViewModel = new();
            allPasswordsViewModel.Database = file;
            allPasswordsViewModel.Password = "admin";
            allPasswordsViewModel.Refresh();
            Assert.Equal(2, allPasswordsViewModel.Passwords.Count);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file));
        }

        [Fact]
        public void ShouldFilterPasswords()
        {
            string file = Path.GetRandomFileName();
            PasswordRepository passwordRepository = new(file, "admin");
            PasswordModel password = new PasswordModel { Username = "admin", Password = "admin", Url = "admin.com" };
            PasswordModel password2 = new PasswordModel { Username = "admin2", Password = "admin2", Url = "admin2.com", Favorite = true };
            PasswordModel password3 = new PasswordModel { Username = "admin3", Password = "admin3", Url = "admin3.com", Favorite = true };
            passwordRepository.Add(password);
            passwordRepository.Add(password2);
            passwordRepository.Add(password3);
            FavoritesViewModel allPasswordsViewModel = new();
            allPasswordsViewModel.Database = file;
            allPasswordsViewModel.Password = "admin";
            allPasswordsViewModel.SearchFilter = "admin2";
            Assert.Single(allPasswordsViewModel.Passwords);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file));
        }
    }
}
