using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using System.Collections.ObjectModel;

namespace PasswordManager.ViewModels
{
    public partial class FavoritesViewModel : ObservableObject, IRefreshable, INavigationAware
    {
        [ObservableProperty]
        private string searchFilter;
        private IPasswordManagementService passwordManagementService;
        private IMessenger passwordListMessenger;

        public FavoritesViewModel(
            IPasswordManagementService passwordManagementService,
            [FromKeyedServices(key: "PasswordList")] IMessenger passwordListMessenger)
        {
            this.passwordManagementService = passwordManagementService;
            this.passwordListMessenger = passwordListMessenger;
        }

        public ObservableCollection<PasswordToShowModel> Passwords { get; set; } = [];

        partial void OnSearchFilterChanged(string value)
        {
            Refresh();
        }

        public void Refresh()
        {
            Passwords.Clear();
            passwordManagementService.GetFilteredPasswords(SearchFilter).Where(p => p.Favorite).ToList().ForEach(Passwords.Add);
            passwordListMessenger.Send(Passwords);
        }

        public void OnNavigatedTo()
        {
            Refresh();
        }

        public void OnNavigatedFrom()
        {
            SearchFilter = string.Empty;
            Passwords.Clear();
            passwordListMessenger.Send(Passwords);
        }
    }
}
