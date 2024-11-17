using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.Utilities;
using PasswordManager.ViewModels.CustomControls;
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
using System.Windows.Media;
using System.Windows.Threading;

namespace PasswordManager.ViewModels
{
    partial class MainViewModel : ObservableObject, IPasswordSettable, IRefreshable
    {
        [ObservableProperty]
        private ObservableObject currentChildView;
        [ObservableProperty]
        private string caption;
        [ObservableProperty]
        private int selectedDb = 0;
        [ObservableProperty]
        private byte[] dBPass;
        [ObservableProperty]
        private bool overlayVisibility;
        [ObservableProperty]
        private List<string> databases = [];
        [ObservableProperty]
        private List<CategoryNodeModel> categories;
        public MainViewModel()
        {
            GetDatabases();
            AutoLocker.SetupTimer();
            BackupCreator backupCreator = new();
            backupCreator.CreateBackupIfNecessary();
        }

        public Brush RandomBrush { get => new SolidColorBrush(Color.FromRgb((byte)Random.Shared.Next(1, 240), (byte)Random.Shared.Next(1, 240), (byte)Random.Shared.Next(1, 240))); }
        
        partial void OnCurrentChildViewChanged(ObservableObject value)
        {
            ((IPasswordSettable)value).DBPass = DBPass;
            ((IDatabaseChangeable)value).Database = Databases[SelectedDb];
            ((IRefreshable)value).Refresh();
            Refresh();
        }

        partial void OnSelectedDbChanged(int value)
        {
            ((IDatabaseChangeable)CurrentChildView).Database = Databases[value];
            ((IRefreshable)CurrentChildView).Refresh();
        }

        public CategoryNodeModel Filter
        {
            get => ((CategoryViewModel)CurrentChildView).Filter;
            set
            {
                if (currentChildView is CategoryViewModel child)
                {
                    child.Filter = value;
                }
            }
        }

        public void GetDatabases()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            List<string> databases = [];
            foreach (var db in Directory.GetFiles(path))
            {
                databases.Add(db[(path + "\\").Length..^".json".Length]);
            }
            if (databases.Count == 0)
            {
                File.Create(Path.Combine(path, "default.json")).Close();
                databases.Add("default");
            }

            Databases = databases;
        }

        public void Refresh()
        {
            GetDatabases();
            PasswordRepository passwordRepository = new(Databases[SelectedDb], DBPass);
            var rootNode = BuildTree(passwordRepository.GetAllPasswords().Select(p => p.CategoryPath).Distinct().Where(p => p != null).ToList());
            Categories=[rootNode];
            ((IRefreshable)currentChildView).Refresh();            
        }

        [RelayCommand]
        private void ShowCategoryView(object obj)
        {
            if (currentChildView is CategoryViewModel)
            {
                return;
            }
            CurrentChildView = new CategoryViewModel();
            Caption = "Categories";
        }

        [RelayCommand]
        private void ShowFavoritesView(object obj)
        {
            CurrentChildView = new FavoritesViewModel();
            Caption = "Favorites";
        }

        [RelayCommand]
        private void ShowAllPasswordsView(object obj)
        {
            CurrentChildView = new AllPasswordsViewModel();
            Caption = "All Passwords";
        }

        [RelayCommand]
        private void ShowPasswordCreationView(object obj)
        {
            OverlayVisibility = true;
            PasswordCreationView passwordCreationView = new();
            ((IPasswordSettable)passwordCreationView.pwdCreator.DataContext).DBPass = DBPass;
            ((IDatabaseChangeable)passwordCreationView.pwdCreator.DataContext).Database = Databases[SelectedDb];
            passwordCreationView.ShowDialog();
            OverlayVisibility = false;
            GetDatabases();
            Refresh();
        }

        [RelayCommand]
        private void ShowPasswordFilePickerDialogueView(object obj)
        {
            OverlayVisibility = true;
            OpenFileDialog openFileDialog = new();
            if (openFileDialog.ShowDialog() == true)
            {
                string passwords = File.ReadAllText(openFileDialog.FileName);
                List<PasswordModel> passwordsToImport;
                if (string.IsNullOrEmpty(passwords))
                {
                    return;
                }

                passwordsToImport = JsonSerializer.Deserialize<List<PasswordModel>>(passwords);
                PasswordRepository passwordRepository = new(Databases[SelectedDb], dBPass);
                foreach (var password in passwordsToImport)
                {
                    passwordRepository.Add(password);
                }
            }

            OverlayVisibility = false;
            ((IRefreshable)CurrentChildView).Refresh();
        }

        private CategoryNodeModel BuildTree(List<string> paths)
        {
            var root = new CategoryNodeModel { Name = "Categories" };
            foreach (var path in paths)
            {
                var parts = path.Split('\\');
                var current = root;
                foreach (var part in parts)
                {
                    var child = current.Children.FirstOrDefault(p => p.Name == part);
                    if (child == null)
                    {
                        child = new CategoryNodeModel { Name = part, Parent = current };
                        current.Children.Add(child);
                    }

                    current = child;
                }
            }

            return root;
        }
    }
}
