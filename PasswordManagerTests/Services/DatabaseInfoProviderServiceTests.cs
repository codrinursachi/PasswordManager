using PasswordManager.State;

namespace PasswordManagerTests.Services
{
    public class DatabaseInfoProviderServiceTests
    {
        [Fact]
        public void ShouldStoreDBPassAndCurrentDatabase()
        {
            DatabaseState databaseInfoProviderService = new();
            databaseInfoProviderService.CurrentDatabase = "Test";
            databaseInfoProviderService.DBPass = [1, 2, 3];
            Assert.Equal("Test", databaseInfoProviderService.CurrentDatabase);
            Assert.Equal([1, 2, 3], databaseInfoProviderService.DBPass);
        }
    }
}
