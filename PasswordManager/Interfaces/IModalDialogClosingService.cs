using System.Windows;

namespace PasswordManager.Interfaces
{
    public interface IModalDialogClosingService
    {
        Stack<Window> ModalDialogs { get; set; }
        void Close();
    }
}
