using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Utilities;
using PasswordManager.Views;
using PasswordManager.Views.Dialogs;
using System.Collections.ObjectModel;

namespace PasswordManager.ViewModels
{
    partial class MainViewModel : ObservableObject, IRefreshable
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
        private IModalDialogClosingService modalDialogClosingService;
        private IDatabaseInfoProviderService databaseInfoProviderService;
        private IPasswordImporterService passwordImporterService;
        private IPasswordManagementService passwordManagementService;
        public MainViewModel(
            IDatabaseInfoProviderService databaseInfoProviderService,
            INavigationService navService,
            IModalDialogProviderService modalDialogOpenerService,
            IModalDialogClosingService modalDialogClosingService,
            IDatabaseStorageService databaseStorageService,
            IPasswordImporterService passwordImporterService,
            IPasswordManagementService passwordManagementService,
            IBackupManagementService backupManagementService)
        {
            this.databaseStorageService = databaseStorageService;
            this.databaseInfoProviderService = databaseInfoProviderService;
            selectedDb = databaseStorageService.Databases[0];
            this.databaseInfoProviderService.CurrentDatabase = selectedDb;
            Navigation = navService;
            this.modalDialogOpenerService = modalDialogOpenerService;
            this.modalDialogClosingService = modalDialogClosingService;
            this.passwordImporterService = passwordImporterService;
            this.passwordManagementService = passwordManagementService;
            AutoLocker.SetupTimer();
            backupManagementService.CreateBackupIfNecessary();
            ShowAllPasswordsViewCommand.Execute(null);
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
            foreach (var db in databaseStorageService.Databases)
            {
                Databases.Add(db);
            }
            SelectedDb = databaseInfoProviderService.CurrentDatabase;
            var rootNode = passwordManagementService.GetPasswordsCategoryRoot();
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
        private void OpenDatabaseManager()
        {
            OverlayVisibility = true;
            var databaseManagerView = modalDialogOpenerService.ProvideModal<DatabaseManagerView>();
            modalDialogClosingService.ModalDialogs.Push(databaseManagerView);
            databaseManagerView.ShowDialog();
            OverlayVisibility = false;
            Refresh();
        }

        [RelayCommand]
        private void ShowPasswordCreationView()
        {
            OverlayVisibility = true;
            var passwordCreationView = modalDialogOpenerService.ProvideModal<PasswordCreationView>();
            modalDialogClosingService.ModalDialogs.Push(passwordCreationView);
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
