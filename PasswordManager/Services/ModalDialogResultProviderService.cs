using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class ModalDialogResultProviderService : IModalDialogResultProviderService
    {
        public bool Result { get; set; }
    }
}
