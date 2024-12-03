using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.CustomControls;
using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using PasswordManager.ViewModels.Dialogs;
using PasswordManager.Views.Dialogs;
using System.Windows;

namespace PasswordManager.Services
{
    public class DialogOverlayService : IDialogOverlayService
    {
        private IUserControlProviderService userControlProviderService;
        public DialogOverlayService(
            IUserControlProviderService userControlProviderService)
        {
            this.userControlProviderService = userControlProviderService;
        }

        public DialogOverlay MainViewOverlay { get; set; }
        public DialogOverlay PasswordEditorOverlay { get; set; }

        public void Close()
        {
            (PasswordEditorOverlay ?? MainViewOverlay).borderOverlay.Visibility = Visibility.Collapsed;
        }

        public void Show<TViewModel>() where TViewModel : ObservableObject
        {
            (PasswordEditorOverlay ?? MainViewOverlay).borderOverlay.Visibility = Visibility.Visible;
            if (typeof(TViewModel) == typeof(DatabaseManagerViewModel))
            {
                MainViewOverlay.dialog.Content = userControlProviderService.ProvideUserControl<DatabaseManagerView>();
            }
            else if (typeof(TViewModel) == typeof(PasswordDeletionDialogViewModel))
            {
                MainViewOverlay.dialog.Content = userControlProviderService.ProvideUserControl<PasswordDeletionDialogView>();
            }
            else if (typeof(TViewModel) == typeof(PasswordModelEditorViewModel))
            {
                MainViewOverlay.dialog.Content = userControlProviderService.ProvideUserControl<PasswordModelEditor>();
                PasswordEditorOverlay = (DialogOverlay)((PasswordModelEditor)MainViewOverlay.dialog.Content).passGenOverlay.Content;
            }
            else if (typeof(TViewModel) == typeof(PasswordGeneratorViewModel))
            {
                (PasswordEditorOverlay ?? MainViewOverlay).dialog.Content = userControlProviderService.ProvideUserControl<PasswordGeneratorView>();
            }
        }
    }
}
