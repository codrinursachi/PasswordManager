using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.CustomControls;
using PasswordManager.DTO;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.Utilities;
using PasswordManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PasswordManager.ViewModels.CustomControls
{
    public partial class PasswordModelEditorViewModel : ObservableObject, IDatabaseChangeable, IPasswordSettable, IPasswordPair
    {
        public ObservableCollection<string> CategoryPaths { get; set; } = [];
        [ObservableProperty]
        private string tags;
        private string database;
        [ObservableProperty]
        private bool overlayVisibility;
        private PasswordRepository passwordRepository;
        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private string password;
        [ObservableProperty]
        private string url;
        [ObservableProperty]
        private DateTime expirationDate = DateTime.Today;
        [ObservableProperty]
        private string categoryPath;
        [ObservableProperty]
        private bool favorite;
        [ObservableProperty]
        private string notes;
        private bool addButtonVisible;
        private bool editButtonVisible;
        [ObservableProperty] 
        private string urlErrorMessage;
        [ObservableProperty]
        private string usernameErrorMessage;
        [ObservableProperty]
        private string passwordErrorMessage;
        private PasswordModel passwordModel;
        [ObservableProperty]
        private IDatabaseStorageService databaseStorageService;
        private IModalDialogProviderService modalDialogProviderService;
        private IDatabaseInfoProviderService databaseInfoProviderService;
        private IPasswordManagementService passwordManagementService;
        public PasswordModelEditorViewModel(
            IDatabaseInfoProviderService databaseInfoProviderService, 
            IDatabaseStorageService databaseStorageService, 
            IModalDialogProviderService modalDialogProviderService,
            IPasswordManagementService passwordManagementService)
        {
            this.passwordManagementService = passwordManagementService;
            this.databaseInfoProviderService = databaseInfoProviderService;
            DBPass=databaseInfoProviderService.DBPass;
            Database = databaseInfoProviderService.CurrentDatabase;
            DatabaseStorageService = databaseStorageService;
            this.modalDialogProviderService = modalDialogProviderService;
            DatabaseItems = [];
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases");
            foreach (var db in Directory.GetFiles(path))
            {
                DatabaseItems.Add(db[(path + "\\").Length..^".json".Length]);
            }

            AddButtonVisible = true;
        }

        public bool AddButtonVisible
        {
            get => addButtonVisible;
            set
            {
                addButtonVisible = value;
                EditButtonVisible = !value;
                OnPropertyChanged(nameof(AddButtonVisible));
            }
        }

        public bool EditButtonVisible
        {
            get => editButtonVisible;
            set
            {
                editButtonVisible = value;
                OnPropertyChanged(nameof(EditButtonVisible));
            }
        }

        public int Id { get; set; }

        public char[] PasswordAsCharArray { get; set; } = [];

        public ObservableCollection<string> CompletedTags { get; set; } = [];

        public string InitialDatabase { get; set; }
        public string Database
        {
            get => database;
            set
            {
                database = value;
                OnPropertyChanged(nameof(Database));
                CategoryPaths.Clear();
                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", value + ".json")))
                {
                    databaseInfoProviderService.CurrentDatabase = value;
                    //passwordRepository = new(value, DBPass);
                    foreach(var pass in (passwordManagementService.GetAllPasswords().Select(p => p.CategoryPath).Distinct().Where(p => p != null)))
                    {
                        CategoryPaths.Add(pass);
                    }
                }
            }
        }

        public List<string> DatabaseItems { get; }
        public Action CloseAction { get; set; }

        public byte[] DBPass { get; set; }

        public PasswordModel PasswordModel
        {
            get => passwordModel;
            set
            {
                passwordModel = value;
                Array.Clear(PasswordAsCharArray);
                Id = passwordModel.Id;
                Url = passwordModel.Url;
                Username = passwordModel.Username;
                PasswordAsCharArray = passwordModel.Password;
                ExpirationDate = passwordModel.ExpirationDate;
                CategoryPath = passwordModel.CategoryPath;
                Favorite = passwordModel.Favorite;
                Notes = passwordModel.Notes;
                Database = databaseInfoProviderService.CurrentDatabase;
                InitialDatabase = Database;
                CompletedTags.Clear();
                if (string.IsNullOrEmpty(passwordModel.Tags))
                {
                    return;
                }
                foreach (var tag in passwordModel.Tags.Split())
                {
                    CompletedTags.Add(tag);
                }
            }
        }

        [RelayCommand]
        public void AddPassword(object obj)
        {
            if (!Validate())
            {
                return;
            }

            string tags = string.Join(" ", CompletedTags);
            PasswordModel newPassword = new() { Username = Username, Password = PasswordAsCharArray, Url = Url, ExpirationDate = ExpirationDate == DateTime.Today ? default : ExpirationDate, CategoryPath = CategoryPath, Tags = tags, Favorite = Favorite, Notes = Notes };
            //PasswordRepository passwordRepository = new(Database, DBPass);
            passwordManagementService.Add(newPassword);
            Array.Fill(PasswordAsCharArray, '0');
            foreach (Window window in App.Current.Windows)
            {
                if (window is PasswordCreationView)
                {
                    window.Close();
                }
            }
        }

        [RelayCommand]
        public void EditPassword(object obj)
        {
            if (PasswordModel == null || !Validate())
            {
                return;
            }

            string tags = string.Join(" ", CompletedTags);
            PasswordModel newPassword = new() { Username = Username, Password = PasswordAsCharArray, Url = Url, ExpirationDate = ExpirationDate == DateTime.Today ? default : ExpirationDate, CategoryPath = CategoryPath, Tags = tags, Favorite = Favorite, Notes = Notes };
            //PasswordRepository passwordRepository = new(Database, DBPass);
            if (InitialDatabase != Database)
            {
                databaseInfoProviderService.CurrentDatabase = InitialDatabase;
                passwordManagementService.Remove(Id);
                databaseInfoProviderService.CurrentDatabase = Database;
                passwordManagementService.Add(newPassword);
            }
            else
            {
                passwordManagementService.Edit(Id, newPassword);
            }

            Password = string.Empty;
            Array.Fill(PasswordAsCharArray, '0');
            ((IRefreshable)obj).Refresh();
        }

        private bool Validate()
        {
            UrlErrorMessage = !string.IsNullOrEmpty(Url) && Uri.IsWellFormedUriString(Url, UriKind.RelativeOrAbsolute) ? string.Empty : "URL must be valid";
            UsernameErrorMessage = !string.IsNullOrEmpty(Username) && Username.Length >= 5 ? string.Empty : "Username must be longer than 4 characters";
            PasswordErrorMessage = PasswordAsCharArray.Length >= 5 ? string.Empty : "Password must be longer than 4 characters";
            return (UrlErrorMessage + UsernameErrorMessage + PasswordErrorMessage).Length == 0;
        }

        [RelayCommand]
        private void ShowPasswordGenerator()
        {
            var passwordGen = modalDialogProviderService.ProvideModal<PasswordGeneratorView>();
            OverlayVisibility = true;
            if (passwordGen.ShowDialog() == true)
            {
                var passGenDataContext = (PasswordGeneratorViewModel)passwordGen.DataContext;
                Password = string.Concat(Enumerable.Repeat('*', passGenDataContext.GeneratedPassword.Length));
                PasswordAsCharArray = passGenDataContext.GeneratedPassword;
            }
            OverlayVisibility = false;
        }
    }
}
