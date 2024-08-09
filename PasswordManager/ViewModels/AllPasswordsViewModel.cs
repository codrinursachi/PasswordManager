﻿using PasswordManager.DTO;
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
    class AllPasswordsViewModel : ViewModelBase
    {
        IPasswordRepository passwordRepository;
        string _searchFilter;
        private DispatcherTimer _timer;

        public AllPasswordsViewModel()
        {
            passwordRepository = new PasswordRepository();
            Passwords = new(passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).Select(p => p.ToPasswordToShow()));
            SetupTimer();
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
        public ObservableCollection<PasswordToShowDTO> Passwords { get; set; }
        public string SearchFilter
        {
            get => _searchFilter;
            set
            {
                _searchFilter = value;
                OnPropertyChanged(nameof(SearchFilter));
                App.Current.Properties["ShouldRefresh"] = true;
                Refresh();
            }
        }

        private void Refresh()
        {
            if ((bool)App.Current.Properties["ShouldRefresh"] == false)
            {
                return;
            }
            App.Current.Properties["ShouldRefresh"] = false;
            HashSet<PasswordToShowDTO> results = new();
            foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).Select(p => p.ToPasswordToShow()))
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
                    results.Add(password);
                }
            }
            if (results.Except(Passwords).Any()||Passwords.Except(results).Any())
            {
                Passwords.Clear();
                foreach (var password in results)
                {
                    Passwords.Add(password);
                }
            }
        }
    }
}
