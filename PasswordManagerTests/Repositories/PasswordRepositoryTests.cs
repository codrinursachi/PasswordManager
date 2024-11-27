using Moq;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManagerTests.Repositories
{
    public class PasswordRepositoryTests
    {
        [Fact]
        public void ShouldAddAndRetrievePasswordsCorrectly()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManagerTests", "PasswordRepository");
            Directory.CreateDirectory(Path.Combine(path, "Databases"));
            var file = Path.GetRandomFileName();
            File.Create(Path.Combine(path, "Databases", file + ".json")).Close();
            var databaseInfoProviderService = new Mock<IDatabaseInfoProviderService>();
            databaseInfoProviderService.Setup(m => m.CurrentDatabase)
                                       .Returns(file);
            databaseInfoProviderService.Setup(m => m.DBPass)
                                       .Returns(ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser));
            var pathProviderService = new Mock<IPathProviderService>();
            pathProviderService.Setup(m => m.ProgramPath)
                               .Returns(path);
            PasswordRepository passwordRepository = new(databaseInfoProviderService.Object, pathProviderService.Object);
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com" };
            passwordRepository.Add(password);
            Assert.NotNull(passwordRepository.GetAllPasswords().FirstOrDefault(p => p.Id == password.Id));
            File.Delete(Path.Combine(path, "Databases", file + ".json"));
        }

        [Fact]
        public void ShouldRetrievePasswordById()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManagerTests", "PasswordRepository");
            Directory.CreateDirectory(Path.Combine(path, "Databases"));
            var file = Path.GetRandomFileName();
            File.Create(Path.Combine(path, "Databases", file + ".json")).Close();
            var databaseInfoProviderService = new Mock<IDatabaseInfoProviderService>();
            databaseInfoProviderService.Setup(m => m.CurrentDatabase)
                                       .Returns(file);
            databaseInfoProviderService.Setup(m => m.DBPass)
                                       .Returns(ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser));
            var pathProviderService = new Mock<IPathProviderService>();
            pathProviderService.Setup(m => m.ProgramPath)
                               .Returns(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManagerTests", "PasswordRepository"));
            PasswordRepository passwordRepository = new(databaseInfoProviderService.Object, pathProviderService.Object);
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com" };
            passwordRepository.Add(password);
            Assert.NotNull(passwordRepository.GetPasswordById(password.Id));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManagerTests", "PasswordRepository", "Databases", file + ".json"));
        }

        [Fact]
        public void ShouldEditPasswordWhenRequested()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManagerTests", "PasswordRepository");
            Directory.CreateDirectory(Path.Combine(path, "Databases"));
            var file = Path.GetRandomFileName();
            File.Create(Path.Combine(path, "Databases", file + ".json")).Close();
            var databaseInfoProviderService = new Mock<IDatabaseInfoProviderService>();
            databaseInfoProviderService.Setup(m => m.CurrentDatabase)
                                       .Returns(file);
            databaseInfoProviderService.Setup(m => m.DBPass)
                                       .Returns(ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser));
            var pathProviderService = new Mock<IPathProviderService>();
            pathProviderService.Setup(m => m.ProgramPath)
                               .Returns(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManagerTests", "PasswordRepository"));
            PasswordRepository passwordRepository = new(databaseInfoProviderService.Object, pathProviderService.Object);
            PasswordModel password = new() { Username = "admin1", Password = "admin".ToCharArray(), Url = "admin.com" };
            PasswordModel passwordEdit = new() { Username = "root", Password = "root".ToCharArray(), Url = "admin.com" };
            passwordRepository.Add(password);
            passwordRepository.Edit(password.Id, passwordEdit);
            Assert.Equal(passwordRepository.GetPasswordById(password.Id).Username, passwordEdit.Username);
            Assert.Equal(passwordRepository.GetPasswordById(password.Id).Password, passwordEdit.Password);
            Assert.Equal(passwordRepository.GetPasswordById(password.Id).Url, passwordEdit.Url);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManagerTests", "PasswordRepository", "Databases", file + ".json"));
        }

        [Fact]
        public void ShouldRemovePasswordWhenRequested()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManagerTests", "PasswordRepository");
            Directory.CreateDirectory(Path.Combine(path, "Databases"));
            var file = Path.GetRandomFileName();
            File.Create(Path.Combine(path, "Databases", file + ".json")).Close();
            var databaseInfoProviderService = new Mock<IDatabaseInfoProviderService>();
            databaseInfoProviderService.Setup(m => m.CurrentDatabase)
                                       .Returns(file);
            databaseInfoProviderService.Setup(m => m.DBPass)
                                       .Returns(ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser));
            var pathProviderService = new Mock<IPathProviderService>();
            pathProviderService.Setup(m => m.ProgramPath)
                               .Returns(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManagerTests", "PasswordRepository"));
            PasswordRepository passwordRepository = new(databaseInfoProviderService.Object, pathProviderService.Object);
            PasswordModel password = new() { Username = "admin2", Password = "admin".ToCharArray(), Url = "admin.com" };
            passwordRepository.Add(password);
            passwordRepository.Remove(password.Id);
            Assert.Null(passwordRepository.GetAllPasswords().FirstOrDefault(p => p == password));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManagerTests", "PasswordRepository", "Databases", file + ".json"));
        }
    }
}
