using System.Windows;

namespace PasswordManager.Interfaces
{
    public interface IModalDialogClosingService
    {
        public Stack<Window> ModalDialogs { get; set; }
        void Close();
    }
}
