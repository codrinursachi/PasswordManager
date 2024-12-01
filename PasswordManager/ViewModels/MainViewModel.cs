using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.CustomControls;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Services;
using PasswordManager.ViewModels.Dialogs;
using PasswordManager.ViewModels.Dialogs;
using PasswordManager.Views;
using PasswordManager.Views.Dialogs;
using System.Collections.ObjectModel;

namespace PasswordManager.ViewModels
{
    public partial class MainViewModel : ObservableObject, IRefreshable
    {
        [ObservableProperty]
        private string caption;
        [ObservableProperty]
        private string selectedDb = string.Empty;
        [ObservableProperty]
        private bool overlayVisibility;
        [ObservableProperty]
        private List<CategoryNodeModel> categories;
        public INavigationService navigationService;
        [ObservableProperty]
        private ObservableCollection<string> databases = [];
        private IDatabaseStorageService databaseStorageService;
        private IDialogOverlayService dialogOverlayService;
        private IDatabaseInfoProviderService databaseInfoProviderService;
        private IPasswordImporterService passwordImporterService;
        private IPasswordManagementService passwordManagementService;
        private IRefreshService refreshService;
        public MainViewModel(
            IDatabaseInfoProviderService databaseInfoProviderService,
            INavigationService navigationService,
            IDialogOverlayService dialogOverlayService,
            IDatabaseStorageService databaseStorageService,
            IPasswordImporterService passwordImporterService,
            IPasswordManagementService passwordManagementService,
            IBackupManagementService backupManagementService,
            IAutoLockerService autoLockerService,
            IRefreshService refreshService)
        {
            this.databaseStorageService = databaseStorageService;
            this.databaseInfoProviderService = databaseInfoProviderService;
            selectedDb = databaseStorageService.Databases[0];
            this.databaseInfoProviderService.CurrentDatabase = selectedDb;
            this.navigationService = navigationService;
            this.dialogOverlayService = dialogOverlayService;
            this.passwordImporterService = passwordImporterService;
            this.passwordManagementService = passwordManagementService;
            autoLockerService.SetupTimer();
            backupManagementService.CreateBackupIfNecessary();
            this.refreshService = refreshService;
            refreshService.Main = this;
        }

        public CategoryNodeModel Filter
        {
            get => ((CategoryViewModel)navigationService.CurrentViewModel).Filter;
            set
            {
                if (navigationService.CurrentViewModel is CategoryViewModel child)
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
            refreshService.RefreshPasswords();
        }

        [RelayCommand]
        private void ShowCategoryView()
        {
            if (navigationService.CurrentViewModel is CategoryViewModel)
            {
                return;
            }

            navigationService.NavigateTo<CategoryViewModel>();
            Caption = "Categories";
        }

        [RelayCommand]
        private void ShowFavoritesView()
        {
            navigationService.NavigateTo<FavoritesViewModel>();
            Caption = "Favorites";
        }

        [RelayCommand]
        private void ShowAllPasswordsView()
        {
            navigationService.NavigateTo<AllPasswordsViewModel>();
            Caption = "All Passwords";
        }

        [RelayCommand]
        private void OpenDatabaseManager()
        {
            dialogOverlayService.Show<DatabaseManagerViewModel>();
        }

        [RelayCommand]
        private void ShowPasswordCreationView()
        {
            dialogOverlayService.Show<PasswordModelEditorViewModel>();
        }

        [RelayCommand]
        private void ShowPasswordFilePickerDialogView()
        {
            passwordImporterService.StartPasswordImport();
            Refresh();
        }
    }
}
