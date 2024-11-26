using PasswordManager.Interfaces;
using System.Windows;

namespace PasswordManager.Services
{
    class ModalDialogClosingService : IModalDialogClosingService
    {
        public Stack<Window> ModalDialogs { get; set; } = [];
        public void Close()
        {
            ModalDialogs.Pop().Close();
        }
    }
}
