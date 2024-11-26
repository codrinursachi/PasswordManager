using PasswordManager.Repositories;

namespace PasswordManagerTests.Repositories
{
    public class UserRepositoryTests
    {
        [Fact]
        public void ShouldAddAndAuthenticateUserCorrectly()
        {
            string file = Path.GetRandomFileName();
            UserRepository userRepository = new(file);
            Assert.True(userRepository.AuthenticateUser("admin".ToCharArray()));
            Assert.False(userRepository.AuthenticateUser("pass".ToCharArray()));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", file));
        }
    }
}
