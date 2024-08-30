using PasswordManager;
using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordManagerTests
{
    public class AllPasswordsViewModelTests
    {
        [Fact]
        public void ShouldStorePasswords()
        {
            string file = Path.GetRandomFileName();
            PasswordRepository passwordRepository = new();
            PasswordModel password = new PasswordModel { Username = "admin", Password = "admin", Url = "admin.com" };
            PasswordModel password2 = new PasswordModel { Username = "admin2", Password = "admin2", Url = "admin2.com" };
            PasswordModel password3 = new PasswordModel { Username = "admin3", Password = "admin3", Url = "admin3.com" };
            passwordRepository.Add(password, "admin", file);
            passwordRepository.Add(password2, "admin", file);
            passwordRepository.Add(password3, "admin", file);
            if (System.Windows.Application.Current == null)
            { new System.Windows.Application { ShutdownMode = ShutdownMode.OnExplicitShutdown }; }
            App.Current.Properties["pass"]="admin";
            AllPasswordsViewModel allPasswordsViewModel = new();
            allPasswordsViewModel.Database = file;
            allPasswordsViewModel.Refresh();
            Assert.Equal(3,allPasswordsViewModel.Passwords.Count);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file));
        }

        [Fact]
        public void ShouldFilterPasswords()
        {
            string file = Path.GetRandomFileName();
            PasswordRepository passwordRepository = new();
            PasswordModel password = new PasswordModel { Username = "admin", Password = "admin", Url = "admin.com" };
            PasswordModel password2 = new PasswordModel { Username = "admin2", Password = "admin2", Url = "admin2.com" };
            PasswordModel password3 = new PasswordModel { Username = "admin3", Password = "admin3", Url = "admin3.com" };
            passwordRepository.Add(password, "admin", file);
            passwordRepository.Add(password2, "admin", file);
            passwordRepository.Add(password3, "admin", file);
            if (System.Windows.Application.Current == null)
            { new System.Windows.Application { ShutdownMode = ShutdownMode.OnExplicitShutdown }; }
            App.Current.Properties["pass"] = "admin";
            AllPasswordsViewModel allPasswordsViewModel = new();
            allPasswordsViewModel.Database = file;
            allPasswordsViewModel.Refresh();
            allPasswordsViewModel.SearchFilter="admin2";
            allPasswordsViewModel.Refresh();
            Assert.Single(allPasswordsViewModel.Passwords);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file));
        }
    }
}
