using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.DTO;
using PasswordManager.DTO.Extensions;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace PasswordManager.ViewModels
{
    public partial class AllPasswordsViewModel : ViewModel, IRefreshable
    {
        [ObservableProperty]
        string searchFilter;
        private ObservableCollection<PasswordToShowDTO> passwords { get; set; } = [];
        private IPasswordManagementService passwordManagementService;
        private IMessenger passwordListMessenger;
        public AllPasswordsViewModel(
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
            passwordManagementService.GetFilteredPasswords(SearchFilter).ForEach(passwords.Add);
            passwordListMessenger.Send(passwords);
        }
    }
}
