using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordManager.Interfaces
{
    public interface IModalDialogClosingService
    {
        public Stack<Window> ModalDialogs { get; set; }
        void Close();
    }
}
