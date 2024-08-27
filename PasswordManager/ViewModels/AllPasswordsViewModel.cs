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
    class AllPasswordsViewModel : ViewModelBase,IRefreshable
    {
        string searchFilter;
        public AllPasswordsViewModel()
        {
            Passwords = new();
            Refresh();
        }
        public ObservableCollection<PasswordToShowDTO> Passwords { get; set; }
        public string SearchFilter
        {
            get => searchFilter;
            set
            {
                searchFilter = value;
                App.Current.Properties["ShouldRefresh"] = true;
                Refresh();
            }
        }

        public void Refresh()
        {
            var passwordRepository = new PasswordRepository();
            App.Current.Properties["ShouldRefresh"] = false;
            Passwords.Clear();
            foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString(),App.Current.Properties["SelectedDb"].ToString()+".json").Select(p => p.ToPasswordToShow()))
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
                    searchData.AddRange(password.Tags.Split(';'));
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
