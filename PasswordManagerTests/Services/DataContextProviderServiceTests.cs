using CommunityToolkit.Mvvm.ComponentModel;
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
            DatabaseInfoProviderService databaseInfoProviderService = new();
            PasswordRepository passwordRepository = new(databaseInfoProviderService);
            PasswordManagementService passwordManagementService = new(passwordRepository);
            DataContextProviderService dataContextProviderService = new((requiredDataContext) => (ObservableObject)Activator.CreateInstance(requiredDataContext, databaseInfoProviderService, passwordManagementService));
            var dataContext = dataContextProviderService.ProvideDataContext<AllPasswordsViewModel>();
            Assert.Equal("PasswordManager.ViewModels.AllPasswordsViewModel", dataContext.ToString());

        }
    }
}
