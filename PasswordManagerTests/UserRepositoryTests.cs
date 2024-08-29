using PasswordManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests
{
    public class UserRepositoryTests
    {
        [Fact]
        public void ShouldAddAndAuthenticateUserCorrectly()
        {
            string file = Path.GetRandomFileName();
            UserRepository userRepository = new(file);
            Assert.True(userRepository.AuthenticateUser("admin"));
            Assert.False(userRepository.AuthenticateUser("pass"));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", file));
        }
    }
}
