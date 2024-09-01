using PasswordManager.Repositories;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests.ViewModels
{
    public class PasswordCreationViewModelTests
    {
        [Fact]
        public void ShouldStorePassword()
        {
            string file = Path.GetRandomFileName();
            PasswordCreationViewModel passwordCreationViewModel = new();
            passwordCreationViewModel.Password = "admin";
            passwordCreationViewModel.Database = file;
            passwordCreationViewModel.Username = "admin";
            passwordCreationViewModel.Password = "admin";
            passwordCreationViewModel.Url = "admin.com";
            passwordCreationViewModel.ExecuteAddPasswordCommand(null);
            PasswordRepository passwordRepository = new(file+".json", "admin");
            Assert.Single(passwordRepository.GetAllPasswords());
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file+".json"));
        }
    }
}
