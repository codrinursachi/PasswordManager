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
    partial class MainViewModel : ObservableObject,IRefreshable
    {
        [ObservableProperty]
        private string caption;
        [ObservableProperty]
        private string selectedDb = string.Empty;
        [ObservableProperty]
        private bool overlayVisibility;
        [ObservableProperty]
        private List<CategoryNodeModel> categories;
        [ObservableProperty]
        private INavigationService navigation;
        [ObservableProperty]
        private ObservableCollection<string> databases = [];
        private IDatabaseStorageService databaseStorageService;
        private IModalDialogProviderService modalDialogOpenerService;
        private IDatabaseInfoProviderService databaseInfoProviderService;
        private IPasswordImporterService passwordImporterService;
        private IPasswordManagementService passwordManagementService;
        public MainViewModel(
            IDatabaseInfoProviderService databaseInfoProviderService, 
            INavigationService navService,
            IModalDialogProviderService modalDialogOpenerService, 
            IDatabaseStorageService databaseStorageService,
            IPasswordImporterService passwordImporterService,
            IPasswordManagementService passwordManagementService)
        {
            this.databaseStorageService = databaseStorageService;
            this.databaseInfoProviderService = databaseInfoProviderService;
            selectedDb = databaseStorageService.Databases[0];
            this.databaseInfoProviderService.CurrentDatabase = selectedDb;
            Navigation = navService;
            this.modalDialogOpenerService=modalDialogOpenerService;
            this.passwordImporterService=passwordImporterService;
            this.passwordManagementService=passwordManagementService;
            AutoLocker.SetupTimer();
            BackupCreator backupCreator = new();
            backupCreator.CreateBackupIfNecessary();
            ShowAllPasswordsViewCommand.Execute(null);
        }

        public Brush RandomBrush { get => new SolidColorBrush(Color.FromRgb((byte)Random.Shared.Next(1, 240), (byte)Random.Shared.Next(1, 240), (byte)Random.Shared.Next(1, 240))); }

        partial void OnSelectedDbChanged(string value)
        {
            if (value == null)
            {
                return;
            }
            databaseInfoProviderService.CurrentDatabase = value;
            var rootNode = passwordManagementService.GetPasswordsCategoryRoot();
            Categories = [rootNode];
            ((IRefreshable)Navigation.CurrentView).Refresh();
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
            databaseStorageService.Refresh();
            Databases.Clear();
            foreach(var db in databaseStorageService.Databases)
            {
                Databases.Add(db);
            }
            SelectedDb = databaseInfoProviderService.CurrentDatabase;
            var rootNode=passwordManagementService.GetPasswordsCategoryRoot();
            Categories = [rootNode];
            ((IRefreshable)Navigation.CurrentView).Refresh();
        }

        [RelayCommand]
        private void ShowCategoryView()
        {
            if (Navigation.CurrentView is CategoryViewModel)
            {
                return;
            }
            Navigation.NavigateTo<CategoryViewModel>();
            Caption = "Categories";
            Refresh();
        }

        [RelayCommand]
        private void ShowFavoritesView()
        {
            Navigation.NavigateTo<FavoritesViewModel>();
            Caption = "Favorites";
            Refresh();
        }

        [RelayCommand]
        private void ShowAllPasswordsView()
        {
            Navigation.NavigateTo<AllPasswordsViewModel>();
            Caption = "All Passwords";
            Refresh();
        }

        [RelayCommand]
        private void ShowPasswordCreationView()
        {
            OverlayVisibility = true;
            var passwordCreationView = modalDialogOpenerService.ProvideModal<PasswordCreationView>();
            passwordCreationView.ShowDialog();
            OverlayVisibility = false;
            Refresh();
        }

        [RelayCommand]
        private void ShowPasswordFilePickerDialogueView()
        {
            OverlayVisibility = true;
            passwordImporterService.StartPasswordImport();
            OverlayVisibility = false;
            Refresh();
        }
    }
}
