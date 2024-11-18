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
        private string caption;
        [ObservableProperty]
        private int selectedDb = 0;
        [ObservableProperty]
        private byte[] dBPass;
        [ObservableProperty]
        private bool overlayVisibility;
        [ObservableProperty]
        private List<CategoryNodeModel> categories;
        [ObservableProperty]
        private INavigationService navigation;
        [ObservableProperty]
        private IDatabaseStorageService databaseStorageService;
        private IModalDialogProviderService modalDialogOpenerService;
        public MainViewModel(byte[] dBPass, INavigationService navService,IModalDialogProviderService modalDialogOpenerService, IDatabaseStorageService databaseStorageService)
        {
            DBPass = dBPass;
            Navigation = navService;
            this.modalDialogOpenerService=modalDialogOpenerService;
            DatabaseStorageService = databaseStorageService;
            AutoLocker.SetupTimer();
            BackupCreator backupCreator = new();
            backupCreator.CreateBackupIfNecessary();
            ShowAllPasswordsViewCommand.Execute(null);
        }

        public Brush RandomBrush { get => new SolidColorBrush(Color.FromRgb((byte)Random.Shared.Next(1, 240), (byte)Random.Shared.Next(1, 240), (byte)Random.Shared.Next(1, 240))); }

        void NavigationExecuted()
        {
            ((IPasswordSettable)Navigation.CurrentView).DBPass = DBPass;
            ((IDatabaseChangeable)Navigation.CurrentView).Database = DatabaseStorageService.Databases[SelectedDb];
            ((IRefreshable)Navigation.CurrentView).Refresh();
            Refresh();
        }

        partial void OnSelectedDbChanged(int value)
        {
            ((IDatabaseChangeable)Navigation.CurrentView).Database = DatabaseStorageService.Databases[value];
            Refresh();
        }

        public CategoryNodeModel Filter
        {
            get => ((CategoryViewModel)Navigation.CurrentView).Filter;
            set
            {
                if (Navigation.CurrentView is CategoryViewModel child)
                {
                    child.Filter = value;
                }
            }
        }

        public void Refresh()
        {
            DatabaseStorageService.Refresh();
            PasswordRepository passwordRepository = new(DatabaseStorageService.Databases[SelectedDb], DBPass);
            var rootNode = BuildTree(passwordRepository.GetAllPasswords().Select(p => p.CategoryPath).Distinct().Where(p => p != null).ToList());
            Categories = [rootNode];
            ((IRefreshable)Navigation.CurrentView).Refresh();
        }

        [RelayCommand]
        private void ShowCategoryView(object obj)
        {
            if (Navigation.CurrentView is CategoryViewModel)
            {
                return;
            }
            Navigation.NavigateTo<CategoryViewModel>();
            Caption = "Categories";
            NavigationExecuted();
        }

        [RelayCommand]
        private void ShowFavoritesView(object obj)
        {
            Navigation.NavigateTo<FavoritesViewModel>();
            Caption = "Favorites";
            NavigationExecuted();
        }

        [RelayCommand]
        private void ShowAllPasswordsView(object obj)
        {
            Navigation.NavigateTo<AllPasswordsViewModel>();
            Caption = "All Passwords";
            NavigationExecuted();
        }

        [RelayCommand]
        private void ShowPasswordCreationView(object obj)
        {
            OverlayVisibility = true;
            var passwordCreationView = modalDialogOpenerService.ProvideModal<PasswordCreationView>();
            ((IPasswordSettable)passwordCreationView.DataContext).DBPass = DBPass;
            ((IDatabaseChangeable)passwordCreationView.DataContext).Database = DatabaseStorageService.Databases[SelectedDb];
            passwordCreationView.ShowDialog();
            OverlayVisibility = false;
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
                PasswordRepository passwordRepository = new(DatabaseStorageService.Databases[SelectedDb], dBPass);
                foreach (var password in passwordsToImport)
                {
                    passwordRepository.Add(password);
                }
            }

            OverlayVisibility = false;
            Refresh();
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
