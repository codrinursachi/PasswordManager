using PasswordManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests.Services
{
    public class DatabaseInfoProviderServiceTests
    {
        [Fact]
        public void ShouldStoreDBPassAndCurrentDatabase()
        {
            DatabaseInfoProviderService databaseInfoProviderService = new();
            databaseInfoProviderService.CurrentDatabase = "Test";
            databaseInfoProviderService.DBPass = [1,2,3];
            Assert.Equal("Test", databaseInfoProviderService.CurrentDatabase);
            Assert.Equal([1,2,3], databaseInfoProviderService.DBPass);
        }
    }
}
