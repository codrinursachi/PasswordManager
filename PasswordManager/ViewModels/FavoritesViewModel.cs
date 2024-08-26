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
    class FavoritesViewModel : ViewModelBase, IStopTimer
    {
        string searchFilter;
        private DispatcherTimer timer;

        public FavoritesViewModel()
        {
            Passwords = new();
            Refresh();
            SetupTimer();
        }
        public ObservableCollection<PasswordToShowDTO> Passwords { get; }
        public string SearchFilter
        {
            get => searchFilter;
            set
            {
                searchFilter = value;
                OnPropertyChanged(nameof(SearchFilter));
                Refresh();
            }
        }

        public void Stop()
        {
            timer.Stop();
        }

        private void SetupTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
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
            foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString(), App.Current.Properties["SelectedDb"].ToString() + ".json").Where(p => p.Favorite).Select(p => p.ToPasswordToShow()))
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
