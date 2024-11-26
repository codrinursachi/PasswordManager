using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Interfaces;

namespace PasswordManager.ViewModels.Dialogs
{
    public partial class PasswordDeletionDialogViewModel : ObservableObject
    {
        private IModalDialogClosingService modalDialogClosingService;
        private IModalDialogResultProviderService modalDialogResultProviderService;
        public PasswordDeletionDialogViewModel(
            IModalDialogClosingService modalDialogClosingService,
            IModalDialogResultProviderService modalDialogResultProviderService)
        {
            this.modalDialogClosingService = modalDialogClosingService;
            this.modalDialogResultProviderService = modalDialogResultProviderService;
        }

        [RelayCommand]
        private void Yes()
        {
            modalDialogResultProviderService.Result = true;
            modalDialogClosingService.Close();
        }

        [RelayCommand]
        private void No()
        {
            modalDialogResultProviderService.Result = false;
            modalDialogClosingService.Close();
        }
    }
}
