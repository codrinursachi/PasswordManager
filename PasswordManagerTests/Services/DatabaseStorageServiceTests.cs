using PasswordManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests.Services
{
    public class DatabaseStorageServiceTests
    {
        [Fact]
        public void ShouldScanAndStoreDatabaseNames()
        {
            DatabaseStorageService databaseStorageService = new("test1");
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", "test1");
            File.Create(Path.Combine(path, "db1.json"));
            File.Create(Path.Combine(path, "db2.json"));
            databaseStorageService.Refresh();
            Assert.Equal(["db1", "db2","default"], databaseStorageService.Databases);
        }
    }
}
