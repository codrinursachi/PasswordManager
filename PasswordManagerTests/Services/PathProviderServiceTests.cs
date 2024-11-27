using PasswordManager.Services;

namespace PasswordManagerTests.Services
{
    public class PathProviderServiceTests
    {
        [Fact]
        public void ShouldReturnCorrectPath()
        {
            PathProviderService pathProviderService = new();
            Assert.Equal(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager"), pathProviderService.ProgramPath);
        }
    }
}
