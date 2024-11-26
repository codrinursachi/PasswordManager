﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.DTO;
using PasswordManager.Interfaces;
using System.Collections.ObjectModel;

namespace PasswordManager.ViewModels
{
    public partial class FavoritesViewModel : ViewModel, IRefreshable
    {
        [ObservableProperty]
        private string searchFilter;
        public ObservableCollection<PasswordToShowDTO> passwords = [];
        private IPasswordManagementService passwordManagementService;
        private IMessenger passwordListMessenger;
        public FavoritesViewModel(
            IPasswordManagementService passwordManagementService,
            [FromKeyedServices(key: "PasswordList")] IMessenger passwordListMessenger)
        {
            this.passwordManagementService = passwordManagementService;
            this.passwordListMessenger = passwordListMessenger;
        }


        partial void OnSearchFilterChanged(string value)
        {
            Refresh();
        }

        public void Refresh()
        {
            passwords.Clear();
            passwordManagementService.GetFilteredPasswords(SearchFilter).Where(p => p.Favorite).ToList().ForEach(passwords.Add);
            passwordListMessenger.Send(passwords);
        }
    }
}
