using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Repositories;
using PasswordManager.Services;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
            DataContextProviderService dataContextProviderService = new((requiredDataContext)=>(ObservableObject)Activator.CreateInstance(requiredDataContext, databaseInfoProviderService, passwordManagementService));
            var dataContext = dataContextProviderService.ProvideDataContext<AllPasswordsViewModel>();
            Assert.Equal("PasswordManager.ViewModels.AllPasswordsViewModel", dataContext.ToString());

        }
    }
}
