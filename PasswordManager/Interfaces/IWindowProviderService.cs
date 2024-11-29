using System.Windows;

namespace PasswordManager.Interfaces
{
    public interface IWindowProviderService
    {
        Window ProvideWindow<TView>() where TView : Window;
    }
}
