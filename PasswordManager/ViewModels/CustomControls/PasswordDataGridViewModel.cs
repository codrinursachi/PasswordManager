using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.DTO;
using PasswordManager.Interfaces;
using PasswordManager.Views.Dialogs;
using System.Collections.ObjectModel;

namespace PasswordManager.ViewModels.CustomControls
{
    public partial class PasswordDataGridViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isEditorVisible;
        [ObservableProperty]
        private PasswordToShowDTO selectedPass;
        [ObservableProperty]
        private ObservableCollection<PasswordToShowDTO> passwordList;
        private IMessenger passwordModelMessenger;
        private IClipboardService clipboardService;
        private IPasswordManagementService passwordManagementService;
        private IWindowProviderService modalDialogProviderService;
        private IModalDialogResultProviderService modalDialogResultProviderService;
        private IModalDialogClosingService modalDialogClosingService;
        public PasswordDataGridViewModel(
            [FromKeyedServices(key: "PasswordModel")] IMessenger passwordModelMessenger,
            [FromKeyedServices(key: "PasswordList")] IMessenger passwordListMessenger,
            IClipboardService clipboardService,
            IPasswordManagementService passwordManagementService,
            IWindowProviderService modalDialogProviderService,
            IModalDialogResultProviderService modalDialogResultProviderService,
            IModalDialogClosingService modalDialogClosingService)
        {
            this.clipboardService = clipboardService;
            this.passwordManagementService = passwordManagementService;
            this.passwordModelMessenger = passwordModelMessenger;
            this.modalDialogProviderService = modalDialogProviderService;
            this.modalDialogResultProviderService = modalDialogResultProviderService;
            this.modalDialogClosingService = modalDialogClosingService;
            passwordListMessenger.Register<PasswordDataGridViewModel, ObservableCollection<PasswordToShowDTO>>(this, (r, m) =>
            {
                r.PasswordList = m;
            });
        }
        partial void OnSelectedPassChanged(PasswordToShowDTO value)
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
            var pwdDeletionDialog = modalDialogProviderService.ProvideWindow<PasswordDeletionDialogView>();
            modalDialogClosingService.ModalDialogs.Push(pwdDeletionDialog);
            pwdDeletionDialog.ShowDialog();
            if (modalDialogResultProviderService.Result)
            {
                modalDialogResultProviderService.Result = false;
                IsEditorVisible = false;
                passwordManagementService.Remove(SelectedPass.Id);
                PasswordList.Remove(SelectedPass);
            }
        }
    }
}
