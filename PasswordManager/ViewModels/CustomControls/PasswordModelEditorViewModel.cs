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
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PasswordManager.ViewModels.CustomControls
{
    public partial class PasswordModelEditorViewModel : ObservableValidator, IPasswordPair
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
        [Required(ErrorMessage ="Field cannot be empty")]
        [MinLength(4, ErrorMessage ="Username must be at least 4 characters")]
        private string username;
        [ObservableProperty]
        [Required(ErrorMessage = "Field cannot be empty")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        private string password;
        [ObservableProperty]
        [Required(ErrorMessage = "Field cannot be empty")]
        [MinLength(4, ErrorMessage = "URL must be at least 4 characters")]
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
        private IModalDialogProviderService modalDialogProviderService;
        private IPasswordManagementService passwordManagementService;
        public PasswordModelEditorViewModel(
            IModalDialogProviderService modalDialogProviderService,
            IPasswordManagementService passwordManagementService)
        {
            this.passwordManagementService = passwordManagementService;
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
            ValidateAllProperties();
            if (HasErrors)
            {
                SetValidationErrorsStrings();
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
            if (PasswordModel == null)
            {
                return;
            }
            ValidateAllProperties();
            if (PasswordModel == null||HasErrors)
            {
                SetValidationErrorsStrings();
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

        private void SetValidationErrorsStrings()
        {
            UsernameErrorMessage = string.Empty;
            UrlErrorMessage = string.Empty;
            PasswordErrorMessage = string.Empty;
            var errorDict = GetErrors().ToDictionary(e => e.MemberNames.First(), e => e.ErrorMessage);
            errorDict.TryGetValue(nameof(Username), out usernameErrorMessage);
            errorDict.TryGetValue(nameof(Url), out urlErrorMessage);
            errorDict.TryGetValue(nameof(Password), out passwordErrorMessage);
            OnPropertyChanged(nameof(UsernameErrorMessage));
            OnPropertyChanged(nameof(UrlErrorMessage));
            OnPropertyChanged(nameof(PasswordErrorMessage));
        }
    }
}
