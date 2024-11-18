using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordManager.Services
{
    public class ModalDialogProviderService:IModalDialogProviderService
    {
        private Func<Type, Window> modalDialogFactory;

        public ModalDialogProviderService(Func<Type, Window> modalDialogFactory)
        {
            this.modalDialogFactory = modalDialogFactory;
        }

        public Window ProvideModal<TView>() where TView : Window
        {
            Window modalDialog = modalDialogFactory.Invoke(typeof(TView));
            return modalDialog;
        }
    }
}
