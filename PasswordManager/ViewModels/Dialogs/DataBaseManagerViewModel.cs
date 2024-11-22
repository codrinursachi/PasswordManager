using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.Dialogs
{
    public partial class DataBaseManagerViewModel : ObservableValidator
    {
        [ObservableProperty]
        private List<string> databases = [];
        [ObservableProperty]
        private string selectedDatabase;
        [ObservableProperty]
        private string newDbName;
        [ObservableProperty]
        private bool databaseSelectionMode = true;
        [ObservableProperty]
        private bool databaseAddingMode;
        [ObservableProperty]
        private string currentMode = "Change Database";
        private IDatabaseStorageService databaseStorageService;
        private IDatabaseInfoProviderService databaseInfoProviderService;
        private IModalDialogClosingService modalDialogClosingService;
        public DataBaseManagerViewModel(
            IDatabaseStorageService databaseStorageService,
            IDatabaseInfoProviderService databaseInfoProviderService,
            IModalDialogClosingService modalDialogClosingService)
        {
            this.databaseStorageService = databaseStorageService;
            this.databaseInfoProviderService = databaseInfoProviderService;
            this.modalDialogClosingService = modalDialogClosingService;
            Databases = databaseStorageService.Databases;
            SelectedDatabase = databaseInfoProviderService.CurrentDatabase;
        }

        [RelayCommand]
        public void ExecuteModeAction()
        {
            if (DatabaseSelectionMode)
            {
                databaseInfoProviderService.CurrentDatabase = SelectedDatabase;
                modalDialogClosingService.Close();
                return;
            }

            databaseStorageService.Add(NewDbName);
            databaseInfoProviderService.CurrentDatabase = NewDbName;
            modalDialogClosingService.Close();
        }

        [RelayCommand]
        public void AddDbMode()
        {
            DatabaseSelectionMode = false;
            DatabaseAddingMode = true;
            CurrentMode = "Add Database";
        }

        [RelayCommand]
        public void RemoveDb()
        {
            databaseStorageService.Remove(SelectedDatabase);
            Databases = databaseStorageService.Databases;
            SelectedDatabase = Databases[0];
            databaseInfoProviderService.CurrentDatabase = SelectedDatabase;
        }
    }
}
