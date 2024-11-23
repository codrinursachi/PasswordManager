using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Views;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

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
        private string urlErrorMessage;
        [ObservableProperty]
        private string usernameErrorMessage;
        [ObservableProperty]
        private string passwordErrorMessage;
        [ObservableProperty]
        private bool isReadOnly;
        [ObservableProperty]
        private bool isEditingEnabled=true;
        [ObservableProperty]
        private string currentAvailableAction;
        [ObservableProperty]
        private int id;
        private IModalDialogProviderService modalDialogProviderService;
        private IModalDialogClosingService modalDialogClosingService;
        private IPasswordManagementService passwordManagementService;
        public PasswordModelEditorViewModel(
            IModalDialogProviderService modalDialogProviderService,
            IModalDialogClosingService modalDialogClosingService,
            [FromKeyedServices("GeneratedPassword")] IMessenger generatedPassMessenger,
            [FromKeyedServices("PasswordModel")] IMessenger passwordModelMessenger,
            IPasswordManagementService passwordManagementService)
        {
            this.passwordManagementService = passwordManagementService;
            this.modalDialogClosingService = modalDialogClosingService;
            this.modalDialogProviderService = modalDialogProviderService;
            generatedPassMessenger.Register<PasswordModelEditorViewModel, char[]>(this, (r,m)=>
            {
                r.Password = string.Concat(Enumerable.Repeat('*', m.Length));
                r.PasswordAsCharArray = m;
            });
            passwordModelMessenger.Register<PasswordModelEditorViewModel, PasswordModel>(this, (r, m) =>
            {
                IsReadOnly = true;
                IsEditingEnabled = false;
                CurrentAvailableAction = "Edit";
                Array.Clear(r.PasswordAsCharArray);
                r.Id = m.Id;
                r.Url = m.Url;
                r.Username = m.Username;
                r.PasswordAsCharArray = m.Password;
                r.Password = "******";
                r.ExpirationDate = m.ExpirationDate;
                r.CategoryPath = m.CategoryPath;
                r.Favorite = m.Favorite;
                r.Notes = m.Notes;
                r.CompletedTags.Clear();
                if (string.IsNullOrEmpty(m.Tags))
                {
                    return;
                }
                foreach (var tag in m.Tags.Split())
                {
                    r.CompletedTags.Add(tag);
                }
            });
            CategoryPaths = passwordManagementService.GetAllPasswords().Select(p => p.CategoryPath).Distinct().Where(p => p != null).ToList();
        }


        public char[] PasswordAsCharArray { get; set; } = [];

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
            modalDialogClosingService.Close();
        }

        [RelayCommand]
        public void AvailableAction(object obj)
        {
            if (CurrentAvailableAction == "Edit")
            {
                EnableEditing();
                return;
            }
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
            passwordManagementService.Edit(Id, newPassword);
            Password = "******";
            Array.Fill(PasswordAsCharArray, '0');
            ((IRefreshable)obj).Refresh();
            DisableEditing();
        }

        public void EnableEditing()
        {
            IsReadOnly = false;
            IsEditingEnabled = true;
            CurrentAvailableAction = "Save";
        }

        [RelayCommand]
        public void DisableEditing()
        {
            IsReadOnly = true;
            IsEditingEnabled = false;
            CurrentAvailableAction = "Edit";
        }

        [RelayCommand]
        private void ShowPasswordGenerator()
        {
            var passwordGen = modalDialogProviderService.ProvideModal<PasswordGeneratorView>();
            modalDialogClosingService.ModalDialogs.Push(passwordGen);
            OverlayVisibility = true;
            passwordGen.ShowDialog();
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
