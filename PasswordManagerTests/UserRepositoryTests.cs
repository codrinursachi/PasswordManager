using PasswordManager.Models;
using PasswordManager.Repositories;

namespace PasswordManagerTests
{
    public class UserRepositoryTests
    {
        [Fact]
        public void ShouldAddAndReturnUsersWhenRequested()
        {
            UserRepository repository = new();
            repository.Add(new UserModel { UserName = "admin", Password = "admin" });
            Assert.NotNull(repository.GetByUsername("admin"));
            Assert.Null(repository.GetByUsername("admin2"));
        }

        [Fact]
        public void ShouldRemoveUserWhenRequested()
        {
            UserRepository repository = new();
            repository.Add(new UserModel { UserName = "admin", Password = "admin" });
            repository.Remove("admin");
            Assert.Null(repository.GetByUsername("admin"));
        }
    }
}