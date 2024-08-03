using PasswordManager.Models;
using PasswordManager.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PasswordManager.ViewModels
{
    class CategoryViewModel : ViewModelBase
    {
        private CategoryNodeModel _filter;
        public CategoryViewModel()
        {
            PasswordRepository passwordRepository = new();
            var rootNode = BuildTree(passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).Select(p => p.CategoryPath).Distinct().Where(p => p != null).ToList());
            Categories = [rootNode];
            Passwords = new(passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).OrderBy(p => p.Url));
        }

        public List<CategoryNodeModel> Categories { get; }
        public ObservableCollection<PasswordModel> Passwords { get; }
        public CategoryNodeModel Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                OnPropertyChanged(nameof(Filter));
                FilterPwd();
            }
        }

        private void FilterPwd()
        {
            Passwords.Clear();
            PasswordRepository passwordRepository = new();
            if (Filter == null || Filter.Parent == null)
            {
                foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).OrderBy(p => p.Url))
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
                path.Add(node.Name + "/");
            }
            path.Reverse();
            foreach (var item in path)
            {
                filter += item;
            }

            foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).Where(p => p.CategoryPath != null && p.CategoryPath.StartsWith(filter)).OrderBy(p => p.Url))
            {
                Passwords.Add(password);
            }
        }

        private CategoryNodeModel BuildTree(List<string> paths)
        {
            var root = new CategoryNodeModel { Name = "Categories" };
            foreach (var path in paths)
            {
                var parts = path.Split('/');
                var current = root;
                foreach (var part in parts)
                {
                    var child = new CategoryNodeModel { Name = part, Parent = current };
                    current.Children.Add(child);
                    current = child;
                }
            }
            return root;
        }
    }
}

