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
using System.Windows.Threading;

namespace PasswordManager.ViewModels
{
    class FavoritesViewModel:ViewModelBase
    {
        string _searchFilter;
        private DispatcherTimer _timer;

        public FavoritesViewModel()
        {
            App.Current.Properties["ShouldRefresh"] = true;
            Passwords = new();
            Refresh();
            SetupTimer();
        }
        public ObservableCollection<PasswordToShowDTO> Passwords { get; }
        public string SearchFilter
        {
            get => _searchFilter;
            set
            {
                _searchFilter = value;
                OnPropertyChanged(nameof(SearchFilter));
                Refresh();
            }
        }

        private void SetupTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            if ((bool)App.Current.Properties["ShouldRefresh"] == false)
            {
                return;
            }
            Passwords.Clear();
            var passwordRepository = new PasswordRepository();
            foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).Where(p=>p.Favorite).Select(p => p.ToPasswordToShow()))
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
                if (string.IsNullOrEmpty(SearchFilter) || searchData.ToHashSet().IsSupersetOf(SearchFilter.Split()))
                {
                    Passwords.Add(password);
                }
            }
        }
    }
}
