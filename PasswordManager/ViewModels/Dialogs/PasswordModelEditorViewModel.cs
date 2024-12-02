using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Views;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace PasswordManager.ViewModels.Dialogs
{
    public partial class PasswordModelEditorViewModel : ObservableValidator, IPasswordPair
    {
        [ObservableProperty]
        private List<string> categoryPaths = [];
        [ObservableProperty]
        private ObservableCollection<string> completedTags = [];
        [ObservableProperty]
        private string tag;
        [ObservableProperty]
        private bool overlayVisibility;
        [ObservableProperty]
        [Required(ErrorMessage = "Field cannot be empty")]
        [MinLength(4, ErrorMessage = "Username must be at least 4 characters")]
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
        private DateTime? expirationDate = null;
        [ObservableProperty]
        private string categoryPath = string.Empty;
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
        private bool addButtonVisible = true;
        [ObservableProperty]
        private bool editButtonVisible = false;
        [ObservableProperty]
        private bool isReadOnly;
        [ObservableProperty]
        private bool isEditingEnabled = true;
        [ObservableProperty]
        private string currentAvailableAction;
        [ObservableProperty]
        private int id;
        private IDialogOverlayService dialogOverlayService;
        private IPasswordManagementService passwordManagementService;
        private IRefreshService refreshService;
        public PasswordModelEditorViewModel(
            IDialogOverlayService dialogOverlayService,
            [FromKeyedServices("GeneratedPassword")] IMessenger generatedPassMessenger,
            [FromKeyedServices("PasswordModel")] IMessenger passwordModelMessenger,
            IPasswordManagementService passwordManagementService,
            IRefreshService refreshService)
        {
            this.passwordManagementService = passwordManagementService;
            this.dialogOverlayService = dialogOverlayService;
            this.refreshService=refreshService;

            generatedPassMessenger.Register<PasswordModelEditorViewModel, char[]>(this, (r, m) =>
            {
                r.Password = string.Concat(Enumerable.Repeat('*', m.Length));
                r.PasswordAsCharArray = m;
            });

            passwordModelMessenger.Register<PasswordModelEditorViewModel, PasswordModel>(this, (r, m) =>
            {
                AddButtonVisible = false;
                EditButtonVisible = true;
                IsReadOnly = true;
                IsEditingEnabled = false;
                CurrentAvailableAction = "Edit";
                Array.Clear(r.PasswordAsCharArray);
                UneditedPass = true;
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
        }

        public bool UneditedPass { get; set; } = true;
        public char[] PasswordAsCharArray { get; set; } = [];
        public Brush RandomBrush { get => new SolidColorBrush(Color.FromRgb((byte)Random.Shared.Next(1, 240), (byte)Random.Shared.Next(1, 240), (byte)Random.Shared.Next(1, 240))); }
        public string CategoryPathEndingChar => CategoryPath.TrimEnd('\\') + "\\";
        partial void OnCategoryPathChanged(string value)
        {
            OnPropertyChanged(nameof(CategoryPathEndingChar));
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
                ExpirationDate = ExpirationDate,
                CategoryPath = CategoryPath,
                Tags = tags,
                Favorite = Favorite,
                Notes = Notes
            };
            passwordManagementService.Add(newPassword);
            Array.Fill(PasswordAsCharArray, '0');
            dialogOverlayService.PasswordEditorOverlay=null;
            dialogOverlayService.Close();
            refreshService.RefreshMain();
        }

        [RelayCommand]
        public void AvailableAction()
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
                ExpirationDate = ExpirationDate,
                CategoryPath = CategoryPath,
                Tags = tags,
                Favorite = Favorite,
                Notes = Notes
            };
            passwordManagementService.Edit(Id, newPassword);
            Password = "******";
            Array.Fill(PasswordAsCharArray, '0');
            refreshService.RefreshMain();
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
            Array.Clear(PasswordAsCharArray);
            UneditedPass = true;
            var pass = passwordManagementService.GetPasswordById(Id);
            Url = pass.Url;
            Username = pass.Username;
            PasswordAsCharArray = pass.Password;
            Password = "******";
            ExpirationDate = pass.ExpirationDate;
            CategoryPath = pass.CategoryPath;
            Favorite = pass.Favorite;
            Notes = pass.Notes;
            CompletedTags.Clear();
            if (string.IsNullOrEmpty(pass.Tags))
            {
                return;
            }
            foreach (var tag in pass.Tags.Split())
            {
                CompletedTags.Add(tag);
            }
        }

        [RelayCommand]
        private void ShowPasswordGenerator()
        {
            dialogOverlayService.Show<PasswordGeneratorViewModel>();
        }

        [RelayCommand]
        private void AddTag()
        {
            if (string.IsNullOrWhiteSpace(Tag) || CompletedTags.Contains("#" + Tag.Trim().Trim('#')))
            {
                return;
            }

            CompletedTags.Add("#" + Tag.Trim().Trim('#'));
            Tag = string.Empty;
        }

        [RelayCommand]
        private void RemoveTag(string tagToRemove)
        {
            CompletedTags.Remove(tagToRemove);
        }

        [RelayCommand]
        private void ClearDate()
        {
            ExpirationDate = null;
        }

        [RelayCommand]
        private void Close()
        {
            dialogOverlayService.PasswordEditorOverlay=null;
            dialogOverlayService.Close();
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
