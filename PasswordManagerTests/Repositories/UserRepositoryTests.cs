using Moq;
using PasswordManager.Interfaces;
using PasswordManager.Repositories;

namespace PasswordManagerTests.Repositories
{
    public class UserRepositoryTests
    {
        [Fact]
        public void ShouldAddAndAuthenticateUserCorrectly()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManagerTests", "UserRepository");
            Directory.CreateDirectory(path);
            var pathProviderService = new Mock<IPathProviderService>();
            pathProviderService.Setup(m => m.ProgramPath)
                               .Returns(path);
            UserRepository userRepository = new(pathProviderService.Object);
            Assert.True(userRepository.AuthenticateUser("admin".ToCharArray()));
            Assert.False(userRepository.AuthenticateUser("pass".ToCharArray()));
            File.Delete(Path.Combine(path, "UserLogin"));
        }
    }
}
