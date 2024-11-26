using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Moq;
using PasswordManager.Interfaces;
using PasswordManager.Repositories;
using PasswordManager.Services;
using PasswordManager.ViewModels;

namespace PasswordManagerTests.Services
{
    public class DataContextProviderServiceTests
    {
        [Fact]
        public void ShouldInstantiateRequiredDataContext()
        {
            var passwordManagementService = new Mock<IPasswordManagementService>();
            var passwordListMessenger = new Mock<IMessenger>();
            DataContextProviderService dataContextProviderService = new((requiredDataContext) => (ObservableObject)Activator.CreateInstance(requiredDataContext, passwordManagementService.Object, passwordListMessenger.Object));
            var dataContext = dataContextProviderService.ProvideDataContext<AllPasswordsViewModel>();
            Assert.Equal("PasswordManager.ViewModels.AllPasswordsViewModel", dataContext.ToString());
        }
    }
}
