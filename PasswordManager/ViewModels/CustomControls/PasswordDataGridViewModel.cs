using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.DTO;
using PasswordManager.DTO.Extensions;
using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;

namespace PasswordManager.ViewModels.CustomControls
{
    partial class PasswordDataGridViewModel : ObservableObject
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
        public PasswordDataGridViewModel(
            [FromKeyedServices(key: "PasswordModel")] IMessenger passwordModelMessenger,
            [FromKeyedServices(key: "PasswordList")] IMessenger passwordListMessenger,
            IClipboardService clipboardService,
            IPasswordManagementService passwordManagementService)
        {
            this.clipboardService = clipboardService;
            this.passwordManagementService = passwordManagementService;
            this.passwordModelMessenger = passwordModelMessenger;
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
            passwordModelMessenger.Send(selectedPass.ToPasswordModel());
        }

        [RelayCommand]
        public void CopyUsername()
        {
            clipboardService.CopyToClipboard(selectedPass.Username);
        }

        [RelayCommand]
        public void CopyPassword()
        {
            clipboardService.TimedCopyToClipboard(new string(passwordManagementService.GetPasswordById(SelectedPass.Id).Password));
        }

        [RelayCommand]
        public void DeletePassword()
        {
            IsEditorVisible = false;
            passwordManagementService.Remove(SelectedPass.Id);
            PasswordList.Remove(SelectedPass);
        }
    }
}
