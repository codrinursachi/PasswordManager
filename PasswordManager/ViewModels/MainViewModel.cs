using Microsoft.Win32;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.Utilities;
using PasswordManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PasswordManager.ViewModels
{
    class MainViewModel : ViewModelBase, IPasswordSettable
    {
        private ViewModelBase currentChildView;
        private string caption;
        private int selectedDb = 0;
        private byte[] dBPass;
        private DispatcherTimer timer;
        private bool overlayVisibility;
        public MainViewModel()
        {
            ShowAllPasswordsViewCommand = new ViewModelCommand(ExecuteShowAllPasswordsViewCommand);
            ShowFavoritesViewCommand = new ViewModelCommand(ExecuteShowFavoritesViewCommand);
            ShowLabelsViewCommand = new ViewModelCommand(ExecuteShowLabelsViewCommand);
            ShowCategoryViewCommand = new ViewModelCommand(ExecuteShowCategoryViewCommand);
            ShowPasswordCreationViewCommand = new ViewModelCommand(ExecuteShowPasswordCreationViewCommand);
            ShowPasswordFilePickerDialogueViewCommand = new ViewModelCommand(ExecuteShowPasswordFilePickerDialogueViewCommand);
            GetDatabases();
            SetupTimer();
            BackupCreator backupCreator = new();
            backupCreator.CreateBackupIfNecessary();
        }

        public ICommand ShowAllPasswordsViewCommand { get; }
        public ICommand ShowFavoritesViewCommand { get; }
        public ICommand ShowLabelsViewCommand { get; }
        public ICommand ShowCategoryViewCommand { get; }
        public ICommand ShowPasswordCreationViewCommand { get; }
        public ICommand ShowPasswordFilePickerDialogueViewCommand { get; }

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
                currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
                ((IPasswordSettable)CurrentChildView).DBPass = DBPass;
                ((IDatabaseChangeable)CurrentChildView).Database = Databases[selectedDb] + ".json";
                ((IRefreshable)CurrentChildView).Refresh();
            }
        }
        public ObservableCollection<string> Databases { get; set; } = new();

        public int SelectedDb
        {
            get => selectedDb;
            set
            {
                selectedDb = value;
                OnPropertyChanged(nameof(SelectedDb));
                if (selectedDb >= 0)
                {
                    ((IDatabaseChangeable)CurrentChildView).Database = Databases[selectedDb] + ".json";
                    ((IRefreshable)CurrentChildView).Refresh();
                }
            }
        }

        public bool OverlayVisibility
        {
            get => overlayVisibility;
            set
            {
                overlayVisibility = value;
                OnPropertyChanged(nameof(OverlayVisibility));
            }
        }

        public byte[] DBPass
        {
            get
            {
                return dBPass;
            }
            set
            {
                dBPass = value;
                ExecuteShowAllPasswordsViewCommand(null);
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
            foreach (var db in Directory.GetFiles(path))
            {
                Databases.Add(db[(path + "\\").Length..^".json".Length]);
            }
            if (Databases.Count == 0)
            {
                File.Create(Path.Combine(path, "default.json")).Close();
                Databases.Add("default");
            }
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

        private void ExecuteShowPasswordCreationViewCommand(object obj)
        {
            OverlayVisibility = true;
            PasswordCreationView passwordCreationView = new();
            ((IPasswordSettable)passwordCreationView.DataContext).DBPass = DBPass;
            ((IDatabaseChangeable)passwordCreationView.DataContext).Database = Databases[SelectedDb];
            passwordCreationView.ShowDialog();
            OverlayVisibility = false;
            GetDatabases();
            SelectedDb = 0;
        }

        private void ExecuteShowPasswordFilePickerDialogueViewCommand(object obj)
        {
            OverlayVisibility = true;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string passwords = File.ReadAllText(openFileDialog.FileName);
                List<PasswordModel> passwordsToImport;
                if (string.IsNullOrEmpty(passwords))
                {
                    return;
                }

                passwordsToImport = JsonSerializer.Deserialize<List<PasswordModel>>(passwords);
                PasswordRepository passwordRepository=new(Databases[SelectedDb]+".json",dBPass);
                foreach(var password in passwordsToImport)
                {
                    passwordRepository.Add(password);
                }
            }

            OverlayVisibility = false;
            ((IRefreshable)CurrentChildView).Refresh();
        }
    }
}
