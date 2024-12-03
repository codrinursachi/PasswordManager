using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.CustomControls;

namespace PasswordManager.Interfaces
{
    public interface IDialogOverlayService
    {
        DialogOverlay MainViewOverlay { get; set; }
        DialogOverlay PasswordEditorOverlay { get; set; }
        void Show<TViewModel>() where TViewModel : ObservableObject;
        void Close();
    }
}
