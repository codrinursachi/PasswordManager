using System.Windows.Controls;

namespace PasswordManager.Interfaces
{
    public interface IUserControlProviderService
    {
        UserControl ProvideUserControl<TUserControl>() where TUserControl : UserControl;
    }
}
