using PasswordManager.Interfaces;
using System.Windows;

namespace PasswordManager.Services
{
    public class ModalDialogClosingService : IModalDialogClosingService
    {
        public Stack<Window> ModalDialogs { get; set; } = [];
        public void Close()
        {
            ModalDialogs.Pop().Close();
        }
    }
}
