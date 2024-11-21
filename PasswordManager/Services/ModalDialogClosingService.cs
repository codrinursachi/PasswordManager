using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordManager.Services
{
    class ModalDialogClosingService : IModalDialogClosingService
    {
        public Stack<Window> ModalDialogs { get; set; } = [];
        public void Close()
        {
            ModalDialogs.Peek().DialogResult = true;
            ModalDialogs.Pop().Close();
        }
    }
}
