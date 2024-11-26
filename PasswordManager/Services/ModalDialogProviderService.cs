using PasswordManager.Interfaces;
using System.Windows;

namespace PasswordManager.Services
{
    public class ModalDialogProviderService : IModalDialogProviderService
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
