using System.Windows;

namespace PasswordManager.Interfaces
{
    public interface IModalDialogProviderService
    {
        Window ProvideModal<TView>() where TView : Window;
    }
}
