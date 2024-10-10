using PasswordManager.CustomControls;
using PasswordManager.Repositories;
using PasswordManager.ViewModels.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests.ViewModels
{
    public class PasswordModelEditorViewModelTests
    {
        [Fact]
        public void ShouldStorePassword()
        {
            string file = Path.GetRandomFileName();
            var dBPass = ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser);
            PasswordModelEditorViewModel passwordCreationViewModel = new()
            {
                PasswordAsCharArray = "admin".ToCharArray(),
                Database = file,
                Username = "admin",
                Url = "admin.com",
                DBPass = dBPass
            };
            passwordCreationViewModel.ExecuteAddPasswordCommand(null);
            PasswordRepository passwordRepository = new(file, dBPass);
            Assert.Single(passwordRepository.GetAllPasswords());
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file + ".json"));
        }
    }
}
