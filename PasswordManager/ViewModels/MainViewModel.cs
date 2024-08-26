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
        private ViewModelBase currentChildView;
        private string caption;
        private int selectedDb;
        private DispatcherTimer timer;
        public MainViewModel()
        {
            ShowAllPasswordsViewCommand = new ViewModelCommand(ExecuteShowAllPasswordsViewCommand);
            ShowFavoritesViewCommand = new ViewModelCommand(ExecuteShowFavoritesViewCommand);
            ShowLabelsViewCommand = new ViewModelCommand(ExecuteShowLabelsViewCommand);
            ShowCategoryViewCommand = new ViewModelCommand(ExecuteShowCategoryViewCommand);
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

        public string Caption
        {
            get => caption;
            set
            {
                caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public ViewModelBase CurrentChildView
        {
            get => currentChildView;
            set
            {
                ((IStopTimer)currentChildView)?.Stop();
                currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
                App.Current.Properties["ShouldRefresh"] = true;
            }
        }
        public ObservableCollection<string> Databases { get; set; }

        public int SelectedDb
        {
            get => selectedDb;
            set
            {
                selectedDb = value;
                OnPropertyChanged(nameof(SelectedDb));
                App.Current.Properties["SelectedDb"] = Databases[selectedDb];
                App.Current.Properties["ShouldRefresh"] = true;
            }
        }

        public void GetDatabases()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Databases.Clear();
            foreach (var db in (Directory.GetFiles(path)))
            {
                Databases.Add(db[(path + "\\").Length..^".json".Length]);
            }
            if(Databases.Count == 0)
            {
                File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", "default.json"));
                Databases.Add("default");
            }
        }

        private void CreateBackupIfNecessary()
        {
            var pathToDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases");
            var pathToBackups = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Backups");
            if (!Directory.Exists(pathToBackups))
            {
                Directory.CreateDirectory(pathToBackups);
            }
            foreach (var db in (Directory.GetFiles(pathToDb)))
            {
                if (CheckBackup(db[(pathToDb + "\\").Length..]))
                {
                    CreateBackup(db);
                }
            }
        }

        private void CreateBackup(string db)
        {
            var pathToDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases");
            var pathToBackups = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Backups");
            File.Copy(db, pathToBackups+"\\" + db[(pathToDb + "\\").Length..] + "" + DateTime.Now.ToShortDateString());
        }

        private bool CheckBackup(string DbName)
        {
            int backupCount = 0;
            DateTime latestBackupTime = default;
            DateTime oldestBackupTime = DateTime.Now;
            string oldestBackup = string.Empty;

            var pathToBackups = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Backups");
            foreach (var db in Directory.GetFiles(pathToBackups))
            {
                if (db[(pathToBackups + "\\").Length..].StartsWith(DbName+""))
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
            timer.Stop();
            timer.Start();
        }

        public void SetupTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            App.Current.Properties["timeout"] = true;
            timer.Stop();
            Application.Current.Shutdown();
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
