using CommunityToolkit.Mvvm.Messaging;
using Moq;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Models.Extensions;
using PasswordManager.ViewModels;

namespace PasswordManagerTests.ViewModels
{
    public class FavoritesViewModelTests
    {
        [Fact]
        public void ShouldStorePasswords()
        {
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com" };
            PasswordModel password2 = new() { Username = "admin2", Password = "admin2".ToCharArray(), Url = "admin2.com", Favorite = true };
            PasswordModel password3 = new() { Username = "admin3", Password = "admin3".ToCharArray(), Url = "admin3.com", Favorite = true };
            var passwordListMessenger = new Mock<IMessenger>();
            var passwordManagementService = new Mock<IPasswordManagementService>();
            passwordManagementService.Setup(m => m.GetFilteredPasswords(null))
                                     .Returns(new List<PasswordToShowModel>([password.ToPasswordToShowModel(), password2.ToPasswordToShowModel(), password3.ToPasswordToShowModel()]));

            FavoritesViewModel FavoritesViewModel = new(passwordManagementService.Object, passwordListMessenger.Object);
            FavoritesViewModel.Refresh();
            Assert.Equal(2, FavoritesViewModel.Passwords.Count);
        }

        [Fact]
        public void ShouldFilterPasswords()
        {
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com" };
            PasswordModel password2 = new() { Username = "admin2", Password = "admin2".ToCharArray(), Url = "admin2.com", Favorite = true };
            PasswordModel password3 = new() { Username = "admin3", Password = "admin3".ToCharArray(), Url = "admin3.com", Favorite = true };
            var passwordListMessenger = new Mock<IMessenger>();
            var passwordManagementService = new Mock<IPasswordManagementService>();
            passwordManagementService.Setup(m => m.GetFilteredPasswords("admin2"))
                                     .Returns(new List<PasswordToShowModel>([password2.ToPasswordToShowModel()]));

            FavoritesViewModel FavoritesViewModel = new(passwordManagementService.Object, passwordListMessenger.Object)
            {
                SearchFilter = "admin2"
            };
            Assert.Single(FavoritesViewModel.Passwords);
        }
    }
}
