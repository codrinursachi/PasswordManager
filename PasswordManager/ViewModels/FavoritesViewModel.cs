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
    class FavoritesViewModel : ViewModelBase, IRefreshable, IDatabaseChangeable
    {
        string searchFilter;
        public FavoritesViewModel()
        {
            Passwords = new();
        }
        public ObservableCollection<PasswordToShowDTO> Passwords { get; }
        public string SearchFilter
        {
            get => searchFilter;
            set
            {
                searchFilter = value;
                Refresh();
            }
        }

        public string Database { get; set; }

        public void Refresh()
        {
            if (Database == null)
            {
                return;
            }
            Passwords.Clear();
            var passwordRepository = new PasswordRepository();
            foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString(), Database+ ".json").Where(p => p.Favorite).Select(p => p.ToPasswordToShowDTO()))
            {
                List<string> searchData = [];
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
                    searchData.AddRange(password.Url.Split());
                }
                if (string.IsNullOrEmpty(SearchFilter) || searchData.ToHashSet().IsSupersetOf(SearchFilter.Split()))
                {
                    Passwords.Add(password);
                }
            }
        }
    }
}
