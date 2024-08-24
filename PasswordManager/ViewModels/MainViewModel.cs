using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PasswordManager.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        private string _caption;
        private int _selectedDb;
        private DispatcherTimer _timer;
        public MainViewModel()
        {
            ShowAllPasswordsViewCommand = new ViewModelCommand(ExecuteShowAllPasswordsViewCommand);
            ShowFavoritesViewCommand = new ViewModelCommand(ExecuteShowFavoritesViewCommand);
            ShowLabelsViewCommand = new ViewModelCommand(ExecuteShowLabelsViewCommand);
            ShowCategoryViewCommand = new ViewModelCommand(ExecuteShowCategoryViewCommand);
            ShowPasswordGeneratorViewCommand = new ViewModelCommand(ExecuteShowPasswordGeneratorCommand);
            Databases = new();
            GetDatabases();
            SelectedDb = 0;
            ExecuteShowAllPasswordsViewCommand(null);
            SetupTimer();
            CreateBackupIfNecessary();
        }

        public ICommand ShowAllPasswordsViewCommand { get; }
        public ICommand ShowFavoritesViewCommand { get; }
        public ICommand ShowLabelsViewCommand { get; }
        public ICommand ShowCategoryViewCommand { get; }
        public ICommand ShowPasswordGeneratorViewCommand { get; }

        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public ViewModelBase CurrentChildView
        {
            get => _currentChildView;
            set
            {
                ((IStopTimer)_currentChildView)?.Stop();
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
                App.Current.Properties["ShouldRefresh"] = true;
            }
        }
        public ObservableCollection<string> Databases { get; set; }

        public int SelectedDb
        {
            get => _selectedDb;
            set
            {
                _selectedDb = value;
                OnPropertyChanged(nameof(SelectedDb));
                App.Current.Properties["SelectedDb"] = Databases[_selectedDb];
                App.Current.Properties["ShouldRefresh"] = true;
            }
        }

        public void GetDatabases()
        {
            Databases.Clear();
            foreach (var db in (Directory.GetFiles(Thread.CurrentPrincipal.Identity.Name)))
            {
                Databases.Add(db[(Thread.CurrentPrincipal.Identity.Name + "\\").Length..]);
            }
        }

        private void CreateBackupIfNecessary()
        {
            foreach (var db in (Directory.GetFiles(Thread.CurrentPrincipal.Identity.Name)))
            {
                if (CheckBackup(db[(Thread.CurrentPrincipal.Identity.Name + "\\").Length..]))
                {
                    CreateBackup(db);
                }
            }
        }

        private void CreateBackup(string db)
        {
            File.Copy(db, Thread.CurrentPrincipal.Identity.Name + "\\Backups\\" + db[(Thread.CurrentPrincipal.Identity.Name + "\\").Length..] + "_" + DateTime.Now.ToShortDateString());
        }

        private bool CheckBackup(string DbName)
        {
            int backupCount = 0;
            DateTime latestBackupTime = default;
            DateTime oldestBackupTime = DateTime.Now;
            string oldestBackup = string.Empty;

            foreach (var db in (Directory.GetFiles(Thread.CurrentPrincipal.Identity.Name + "\\Backups\\")))
            {
                if (db[(Thread.CurrentPrincipal.Identity.Name + "\\Backups\\").Length..].StartsWith(DbName))
                {
                    backupCount++;
                    latestBackupTime = File.GetCreationTime(db) > latestBackupTime ? File.GetCreationTime(db) : latestBackupTime;
                    if (File.GetCreationTime(db) < oldestBackupTime)
                    {
                        oldestBackupTime = File.GetCreationTime(DbName);
                        oldestBackup = db;
                    }
                }
            }

            if (backupCount >= 5)
            {
                File.Delete(oldestBackup);
            }

            return latestBackupTime < DateTime.Now - TimeSpan.FromDays(7);
        }

        public void OnActivity(object? sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        public void SetupTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(60);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            App.Current.Properties["timeout"] = true;
            _timer.Stop();
            Application.Current.Shutdown();
        }

        private void ExecuteShowPasswordGeneratorCommand(object obj)
        {
            var PasswordGen = new PasswordGeneratorView();
            PasswordGen.Load();
        }

        private void ExecuteShowCategoryViewCommand(object obj)
        {
            CurrentChildView = new CategoryViewModel();
            Caption = "Categories";
        }

        private void ExecuteShowLabelsViewCommand(object obj)
        {
            CurrentChildView = new TagsViewModel();
            Caption = "Tags";
        }

        private void ExecuteShowFavoritesViewCommand(object obj)
        {
            CurrentChildView = new FavoritesViewModel();
            Caption = "Favorites";
        }

        private void ExecuteShowAllPasswordsViewCommand(object obj)
        {
            CurrentChildView = new AllPasswordsViewModel();
            Caption = "All Passwords";
        }
    }
}
