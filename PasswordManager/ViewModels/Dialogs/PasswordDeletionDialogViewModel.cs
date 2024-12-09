using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Interfaces;

namespace PasswordManager.ViewModels.Dialogs
{
    public partial class PasswordDeletionDialogViewModel : ObservableObject
    {
        private IDialogOverlayService dialogOverlayService;
        private IPasswordDeletionService passwordDeletionService;
        private IRefreshService refreshService;

        public PasswordDeletionDialogViewModel(
            IDialogOverlayService dialogOverlayService,
            IPasswordDeletionService passwordDeletionService,
            IRefreshService refreshService)
        {
            this.dialogOverlayService = dialogOverlayService;
            this.passwordDeletionService = passwordDeletionService;
            this.refreshService = refreshService;
        }

        [RelayCommand]
        public void Yes()
        {
            passwordDeletionService.Delete();
            refreshService.RefreshMain();
            refreshService.RefreshPasswords();
            dialogOverlayService.Close();
        }

        [RelayCommand]
        public void No()
        {
            dialogOverlayService.Close();
        }
    }
}
