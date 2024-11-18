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
    public partial class CategoryViewModel : ViewModel, IRefreshable, IDatabaseChangeable, IPasswordSettable
    {
        [ObservableProperty]
        private CategoryNodeModel filter;
        private IDatabaseInfoProviderService databaseInfoProviderService;
        private IPasswordManagementService passwordManagementService;
        public CategoryViewModel(IDatabaseInfoProviderService databaseInfoProviderService, IPasswordManagementService passwordManagementService)
        {
            this.databaseInfoProviderService = databaseInfoProviderService;
            this.passwordManagementService = passwordManagementService;
        }

        public ObservableCollection<PasswordToShowDTO> Passwords { get; set; } = [];
        
        public string Database { get; set; }
        public byte[] DBPass { get; set; }
        partial void OnFilterChanged(CategoryNodeModel value)
        {
            Refresh();
        }
        public void Refresh()
        {
            FilterPass();
        }
        private void FilterPass()
        {
            Passwords.Clear();
            //PasswordRepository passwordRepository = new(databaseInfoProviderService.CurrentDatabase, databaseInfoProviderService.DBPass);
            if (Filter == null || Filter.Parent == null)
            {
                foreach (var password in passwordManagementService.GetAllPasswords().Select(p => p.ToPasswordToShowDTO()))
                {
                    Passwords.Add(password);
                }

                return;
            }

            string filter = string.Empty;
            List<string> path = [];
            var node = Filter;
            path.Add(node.Name);
            while (node.Parent.Name != "Categories")
            {
                node = node.Parent;
                path.Add(node.Name + "\\");
            }

            path.Reverse();
            foreach (var item in path)
            {
                filter += item;
            }

            foreach (var password in passwordManagementService.GetAllPasswords().Where(p => p.CategoryPath != null && p.CategoryPath.StartsWith(filter) && (string.IsNullOrEmpty(p.CategoryPath[filter.Length..]) || p.CategoryPath[filter.Length] == '\\')).Select(p => p.ToPasswordToShowDTO()))
            {
                Passwords.Add(password);
            }
        }
    }
}

