using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests
{
    public class TagsViewModelTests
    {
        [Fact]
        public void ShouldStorePasswords()
        {
            string file = Path.GetRandomFileName();
            PasswordRepository passwordRepository = new(file, "admin");
            PasswordModel password = new PasswordModel { Username = "admin", Password = "admin", Url = "admin.com", CategoryPath = "admin\\1" };
            PasswordModel password2 = new PasswordModel { Username = "admin2", Password = "admin2", Url = "admin2.com", CategoryPath = "admin\\2" };
            PasswordModel password3 = new PasswordModel { Username = "admin3", Password = "admin3", Url = "admin3.com", CategoryPath = "admin\\3" };
            passwordRepository.Add(password);
            passwordRepository.Add(password2);
            passwordRepository.Add(password3);
            TagsViewModel tagsViewModel = new();
            tagsViewModel.Database = file;
            tagsViewModel.Password = "admin";
            tagsViewModel.Refresh();
            Assert.Equal(3, tagsViewModel.Passwords.Count);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file));
        }
    }
}
