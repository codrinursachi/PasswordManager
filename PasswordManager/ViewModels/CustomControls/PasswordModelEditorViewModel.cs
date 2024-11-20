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
    public partial class PasswordModelEditorViewModel : ObservableObject, IPasswordPair
    {
        [ObservableProperty]
        private List<string> categoryPaths = [];
        [ObservableProperty]
        private ObservableCollection<string> completedTags = [];
        [ObservableProperty]
        private string tags;
        [ObservableProperty]
        private bool overlayVisibility;
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
        [ObservableProperty]
        private bool addButtonVisible = true;
        [ObservableProperty]
        private bool editButtonVisible = false;
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
            DatabaseStorageService = databaseStorageService;
            this.modalDialogProviderService = modalDialogProviderService;
            CategoryPaths = passwordManagementService.GetAllPasswords().Select(p => p.CategoryPath).Distinct().Where(p => p != null).ToList();
        }

        partial void OnAddButtonVisibleChanged(bool value)
        {
            EditButtonVisible = !value;
        }

        public int Id { get; set; }

        public char[] PasswordAsCharArray { get; set; } = [];

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
            PasswordModel newPassword = new()
            {
                Username = Username,
                Password = PasswordAsCharArray,
                Url = Url,
                ExpirationDate = ExpirationDate == DateTime.Today ? default : ExpirationDate,
                CategoryPath = CategoryPath,
                Tags = tags,
                Favorite = Favorite,
                Notes = Notes
            };
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
            PasswordModel newPassword = new()
            {
                Username = Username,
                Password = PasswordAsCharArray,
                Url = Url,
                ExpirationDate = ExpirationDate == DateTime.Today ? default : ExpirationDate,
                CategoryPath = CategoryPath,
                Tags = tags,
                Favorite = Favorite,
                Notes = Notes
            };
            passwordManagementService.Edit(Id, newPassword);
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
