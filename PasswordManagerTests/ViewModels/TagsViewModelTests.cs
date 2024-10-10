using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests.ViewModels
{
    public class TagsViewModelTests
    {
        [Fact]
        public void ShouldStorePasswords()
        {
            string file = Path.GetRandomFileName();
            PasswordRepository passwordRepository = new(file, ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser));
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com", Tags="#tag1 #tag2" };
            PasswordModel password2 = new() { Username = "admin2", Password = "admin2".ToCharArray(), Url = "admin2.com", Tags = "#tag2 #tag3" };
            PasswordModel password3 = new() { Username = "admin3", Password = "admin3".ToCharArray(), Url = "admin3.com", Tags = "#tag3 #tag4" };
            passwordRepository.Add(password);
            passwordRepository.Add(password2);
            passwordRepository.Add(password3);
            TagsViewModel TagsViewModel = new()
            {
                Database = file,
                DBPass = ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser)
            };
            TagsViewModel.Refresh();
            Assert.Equal(3, TagsViewModel.Passwords.Count);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file + ".json"));
        }

        [Fact]
        public void ShouldFilterPasswords()
        {
            string file = Path.GetRandomFileName();
            PasswordRepository passwordRepository = new(file, ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser));
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com", Tags = "#tag1 #tag2" };
            PasswordModel password2 = new() { Username = "admin2", Password = "admin2".ToCharArray(), Url = "admin2.com", Tags = "#tag2 #tag3" };
            PasswordModel password3 = new() { Username = "admin3", Password = "admin3".ToCharArray(), Url = "admin3.com", Tags = "#tag3 #tag4" };
            passwordRepository.Add(password);
            passwordRepository.Add(password2);
            passwordRepository.Add(password3);
            TagsViewModel TagsViewModel = new()
            {
                Database = file,
                DBPass = ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser),
                Filter = ["#tag2"]
            };
            Assert.Equal(2,TagsViewModel.Passwords.Count);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file + ".json"));
        }
    }
}
