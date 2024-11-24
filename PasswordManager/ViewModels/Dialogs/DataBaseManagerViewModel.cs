using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordManager.ViewModels.Dialogs
{
    public partial class DatabaseManagerViewModel : ObservableValidator
    {
        [ObservableProperty]
        private ObservableCollection<string> databases = [];
        [ObservableProperty]
        private string selectedDatabase;
        [ObservableProperty]
        [Required]
        private string newDbName;
        [ObservableProperty]
        private bool databaseSelectionMode = true;
        [ObservableProperty]
        private bool databaseAddingMode;
        [ObservableProperty]
        private string currentMode = "OK";
        [ObservableProperty]
        private bool messageToDisplay;
        [ObservableProperty]
        private string message;
        private IDatabaseStorageService databaseStorageService;
        private IDatabaseInfoProviderService databaseInfoProviderService;
        private IModalDialogClosingService modalDialogClosingService;
        public DatabaseManagerViewModel(
            IDatabaseStorageService databaseStorageService,
            IDatabaseInfoProviderService databaseInfoProviderService,
            IModalDialogClosingService modalDialogClosingService)
        {
            this.databaseStorageService = databaseStorageService;
            this.databaseInfoProviderService = databaseInfoProviderService;
            this.modalDialogClosingService = modalDialogClosingService;
            databaseStorageService.Databases.ForEach(Databases.Add);
            SelectedDatabase = databaseInfoProviderService.CurrentDatabase;
        }

        partial void OnSelectedDatabaseChanged(string value)
        {
            MessageToDisplay = false;
            DatabaseSelectionMode = true;
            CurrentMode = "Ok";
        }

        [RelayCommand]
        public void CurrentModeAction()
        {
            if (DatabaseAddingMode)
            {
                if (Databases.Contains(NewDbName))
                {
                    Message = "Database already exists.";
                    MessageToDisplay = true;
                    return;
                }

                MessageToDisplay = false;
                databaseStorageService.Add(NewDbName);
                databaseInfoProviderService.CurrentDatabase = NewDbName;
                Databases.Add(NewDbName);
                SelectedDatabase = NewDbName;
                DatabaseSelectionMode = true;
                DatabaseAddingMode = false;
                return;
            }

            if (MessageToDisplay)
            {
                databaseStorageService.Remove(SelectedDatabase);
                Databases.Remove(SelectedDatabase);
                SelectedDatabase = Databases[0];
                databaseInfoProviderService.CurrentDatabase = SelectedDatabase;
                return;
            }
            if (DatabaseSelectionMode)
            {
                databaseInfoProviderService.CurrentDatabase = SelectedDatabase;
                modalDialogClosingService.Close();
                return;
            }
        }

        [RelayCommand]
        public void EnterDatabaseAddMode()
        {
            DatabaseSelectionMode = false;
            DatabaseAddingMode = true;
            CurrentMode = "Add Database";
        }

        [RelayCommand]
        public void EnterDatabaseDeleteMode()
        {
            Message = "This will remove the selected database. Are you sure?";
            MessageToDisplay = true;
            CurrentMode = "Delete database";
        }

        [RelayCommand]
        public void Cancel()
        {
            if (DatabaseAddingMode)
            {
                DatabaseSelectionMode = true;
                DatabaseAddingMode = false;
                CurrentMode = "OK";
                MessageToDisplay = false;
                return;
            }

            if (MessageToDisplay)
            {
                DatabaseAddingMode = true;
                MessageToDisplay = false;
                CurrentMode = "Ok";
                return;
            }

            modalDialogClosingService.Close();
        }
    }
}
