using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.ViewModels.Dialogs;
using PasswordManager.Views.Dialogs;
using System.Collections.ObjectModel;

namespace PasswordManager.ViewModels
{
    public partial class PasswordDataGridViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isEditorVisible;
        [ObservableProperty]
        private PasswordToShowModel selectedPass;
        [ObservableProperty]
        private ObservableCollection<PasswordToShowModel> passwordList;
        private IMessenger passwordModelMessenger;
        private IClipboardService clipboardService;
        private IPasswordManagementService passwordManagementService;
        private IDialogOverlayService dialogOverlayService;
        private IPasswordDeletionService passwordDeletionService;
        public PasswordDataGridViewModel(
            [FromKeyedServices(key: "PasswordModel")] IMessenger passwordModelMessenger,
            [FromKeyedServices(key: "PasswordList")] IMessenger passwordListMessenger,
            IClipboardService clipboardService,
            IPasswordManagementService passwordManagementService,
            IDialogOverlayService dialogOverlayService,
            IPasswordDeletionService passwordDeletionService)
        {
            this.clipboardService = clipboardService;
            this.passwordDeletionService = passwordDeletionService;
            this.passwordManagementService = passwordManagementService;
            this.passwordModelMessenger = passwordModelMessenger;
            this.dialogOverlayService = dialogOverlayService;

            passwordListMessenger.Register<PasswordDataGridViewModel, ObservableCollection<PasswordToShowModel>>(this, (r, m) =>
            {
                r.PasswordList = m;
                IsEditorVisible = false;
            });
        }
        partial void OnSelectedPassChanged(PasswordToShowModel value)
        {
            if (value == null)
            {
                return;
            }

            IsEditorVisible = true;
            passwordModelMessenger.Send(passwordManagementService.GetPasswordById(SelectedPass.Id));
        }

        [RelayCommand]
        public void CopyUsername()
        {
            clipboardService.CopyToClipboard(SelectedPass.Username);
        }

        [RelayCommand]
        public void CopyPassword()
        {
            clipboardService.TimedCopyToClipboard(new string(passwordManagementService.GetPasswordById(SelectedPass.Id).Password));
        }

        [RelayCommand]
        public void DeletePassword()
        {
            passwordDeletionService.Id = SelectedPass.Id;
            dialogOverlayService.Show<PasswordDeletionDialogViewModel>();
        }
    }
}
