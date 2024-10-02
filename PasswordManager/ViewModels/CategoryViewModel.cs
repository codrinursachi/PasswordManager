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
    public class CategoryViewModel : ViewModelBase, IRefreshable, IDatabaseChangeable, IPasswordSettable
    {
        private CategoryNodeModel filter;

        public ObservableCollection<PasswordToShowDTO> Passwords { get; set; } = new();
        public CategoryNodeModel Filter
        {
            get => filter;
            set
            {
                filter = value;
                FilterPass();
            }
        }

        public string Database { get; set; }
        public byte[] DBPass { get; set; }

        public void Refresh()
        {
            FilterPass();
        }
        private void FilterPass()
        {
            Passwords.Clear();
            PasswordRepository passwordRepository = new(Database, DBPass);
            if (Filter == null || Filter.Parent == null)
            {
                foreach (var password in passwordRepository.GetAllPasswords().Select(p => p.ToPasswordToShowDTO()))
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

            foreach (var password in passwordRepository.GetAllPasswords().Where(p => p.CategoryPath != null && p.CategoryPath.StartsWith(filter) && (string.IsNullOrEmpty(p.CategoryPath[filter.Length..]) || p.CategoryPath[filter.Length] == '\\')).Select(p => p.ToPasswordToShowDTO()))
            {
                Passwords.Add(password);
            }
        }
    }
}

