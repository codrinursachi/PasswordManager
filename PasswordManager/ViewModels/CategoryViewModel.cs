using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using System.Collections.ObjectModel;

namespace PasswordManager.ViewModels
{
    public partial class CategoryViewModel : ObservableObject, IRefreshable, INavigationAware
    {
        [ObservableProperty]
        private CategoryNodeModel filter;
        private IPasswordManagementService passwordManagementService;
        private IMessenger passwordListMessenger;

        public CategoryViewModel(
            IPasswordManagementService passwordManagementService,
            [FromKeyedServices("PasswordList")] IMessenger passwordListMessenger)
        {
            this.passwordManagementService = passwordManagementService;
            this.passwordListMessenger = passwordListMessenger;
        }

        public ObservableCollection<PasswordToShowModel> Passwords { get; set; } = [];

        partial void OnFilterChanged(CategoryNodeModel value)
        {
            Refresh();
        }

        public void Refresh()
        {
            Passwords.Clear();
            if (Filter == null || Filter.Parent == null)
            {
                passwordManagementService.GetAllPasswords().ForEach(Passwords.Add);
                passwordListMessenger.Send(Passwords);
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

            foreach (var password in passwordManagementService.GetAllPasswords().Where(
                p => p.CategoryPath != null &&
                p.CategoryPath.StartsWith(filter) &&
                (string.IsNullOrEmpty(p.CategoryPath[filter.Length..]) || p.CategoryPath[filter.Length] == '\\')))
            {
                Passwords.Add(password);
            }

            passwordListMessenger.Send(Passwords);
        }

        public void OnNavigatedTo()
        {
            Refresh();
        }

        public void OnNavigatedFrom()
        {
            Filter = null;
            Passwords.Clear();
            passwordListMessenger.Send(Passwords);
        }
    }
}

