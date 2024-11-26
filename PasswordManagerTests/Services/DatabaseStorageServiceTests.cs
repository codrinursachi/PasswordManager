using Moq;
using PasswordManager.Interfaces;
using PasswordManager.Services;

namespace PasswordManagerTests.Services
{
    public class DatabaseStorageServiceTests
    {
        [Fact]
        public void ShouldScanAndStoreDatabaseNames()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManagerTests", "DatabaseStorageService");
            Directory.CreateDirectory(Path.Combine(path,"Databases"));
            var pathProviderService = new Mock<IPathProviderService>();
            pathProviderService.Setup(m => m.ProgramPath)
                               .Returns(path);
            DatabaseStorageService databaseStorageService = new(pathProviderService.Object);
            var path2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManagerTests", "DatabaseStorageService", "Databases");
            File.Create(Path.Combine(path2, "db1.json"));
            File.Create(Path.Combine(path2, "db2.json"));
            databaseStorageService.Refresh();
            Assert.Equal(["db1", "db2", "default"], databaseStorageService.Databases);
        }
    }
}
