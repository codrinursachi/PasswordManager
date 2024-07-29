using PasswordManager.Models;
using PasswordManager.Repositories;
using System.Net;

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

        [Fact]
        public void ShouldAuthenticateUserWhenRequested()
        {
            UserRepository repository = new();
            repository.Add(new UserModel { UserName = "admin", Password = "admin" });
            Assert.True(repository.AuthenticateUser(new NetworkCredential("admin", "admin")));
        }

        [Fact]
        public void ShouldEditUserPasswordWhenRequested()
        {
            UserRepository repository = new();
            repository.Add(new UserModel { UserName = "admin", Password = "admin" });
            repository.Edit(new UserModel { UserName="admin", Password="root"});
            Assert.True(repository.AuthenticateUser(new NetworkCredential("admin", "root")));
        }
    }
}