using PasswordManager.DTO;
using PasswordManager.DTO.Extensions;
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
    class AllPasswordsViewModel : ViewModelBase
    {
        IPasswordRepository passwordRepository;
        string _searchFilter;
        public AllPasswordsViewModel()
        {
            RefreshCommand = new ViewModelCommand(ExecuteRefreshCommand);
            passwordRepository = new PasswordRepository();
            Passwords = new(passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).OrderBy(p => p.Url).Select(p => p.ToPasswordToShow()));
        }
        public ICommand RefreshCommand { get; }
        public ObservableCollection<PasswordToShowDTO> Passwords { get; private set; }
        public string SearchFilter
        {
            get => _searchFilter;
            set
            {
                _searchFilter = value;
                OnPropertyChanged(nameof(SearchFilter));
                ExecuteRefreshCommand(null);
            }
        }

        private void ExecuteRefreshCommand(object obj)
        {
            Passwords.Clear();
            foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).OrderBy(p => p.Url).Select(p => p.ToPasswordToShow()))
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
                    searchData.AddRange(password.Url.Split());
                }
                if (SearchFilter == null||searchData.ToHashSet().IsSupersetOf(SearchFilter.Split()))
                {
                    Passwords.Add(password);
                }
            }
        }
    }
}
