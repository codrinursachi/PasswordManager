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
    class CategoryViewModel : ViewModelBase, IRefreshable, IDatabaseChangeable
    {
        private CategoryNodeModel filter;

        public ObservableCollection<CategoryNodeModel> Categories { get; set; } = new();
        public ObservableCollection<PasswordToShowDTO> Passwords { get; } = new();
        public CategoryNodeModel Filter
        {
            get => filter;
            set
            {
                filter = value;
                OnPropertyChanged(nameof(Filter));
                FilterPass();
            }
        }

        public string Database { get; set; }

        public void Refresh()
        {
            PasswordRepository passwordRepository = new();
            Categories.Clear();
            var rootNode = BuildTree(passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString(), Database).Select(p => p.CategoryPath).Distinct().Where(p => p != null).ToList());
            Categories.Add(rootNode);
            FilterPass();
        }

        private void FilterPass()
        {
            Passwords.Clear();
            PasswordRepository passwordRepository = new();
            if (Filter == null || Filter.Parent == null)
            {
                foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString(), Database).Select(p => p.ToPasswordToShowDTO()))
                {
                    Passwords.Add(password);
                }

                return;
            }

            string filter = string.Empty;
            List<string> path = new();
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

            foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString(), Database).Where(p => p.CategoryPath != null && p.CategoryPath.StartsWith(filter) && (string.IsNullOrEmpty(p.CategoryPath[filter.Length..]) || p.CategoryPath[filter.Length] == '\\')).Select(p => p.ToPasswordToShowDTO()))
            {
                Passwords.Add(password);
            }
        }

        private CategoryNodeModel BuildTree(List<string> paths)
        {
            var root = new CategoryNodeModel { Name = "Categories" };
            foreach (var path in paths)
            {
                var parts = path.Split('\\');
                var current = root;
                foreach (var part in parts)
                {
                    var child = current.Children.FirstOrDefault(p => p.Name == part);
                    if (child == null)
                    {
                        child = new CategoryNodeModel { Name = part, Parent = current };
                        current.Children.Add(child);
                    }

                    current = child;
                }
            }

            return root;
        }

    }
}

