using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class AllPasswordsViewModel : ViewModel, IRefreshable, IDatabaseChangeable, IPasswordSettable
    {
        [ObservableProperty]
        string searchFilter;
        public ObservableCollection<PasswordToShowDTO> Passwords { get; set; } = [];
        private IDatabaseInfoProviderService databaseInfoProviderService;
        private IPasswordManagementService passwordManagementService;
        public AllPasswordsViewModel(IDatabaseInfoProviderService databaseInfoProviderService, IPasswordManagementService passwordManagementService)
        {
            this.passwordManagementService = passwordManagementService;
            this.databaseInfoProviderService = databaseInfoProviderService;
        }

        public string Database { get; set; }
        public byte[] DBPass { get; set; }

        partial void OnSearchFilterChanged(string value)
        {
            Refresh();
        }

        public void Refresh()
        {
            Passwords.Clear();
            foreach (var password in passwordManagementService.GetAllPasswords().Select(p => p.ToPasswordToShowDTO()))
            {
                List<string> searchData = [];
                if (password.Username != null)
                {
                    searchData.Add(password.Username);
                }
                if (password.CategoryPath != null)
                {
                    searchData.AddRange(password.CategoryPath.Split('/'));
                }
                if (password.Notes != null)
                {
                    searchData.AddRange(password.Notes.Split());
                }
                if (password.Tags != null)
                {
                    searchData.AddRange(password.Tags.Split());
                }
                if (password.Url != null)
                {
                    searchData.Add(password.Url);
                }
                if (string.IsNullOrEmpty(SearchFilter) || searchData.ToHashSet().IsSupersetOf(SearchFilter.Split()))
                {
                    Passwords.Add(password);
                }
            }
        }
    }
}
