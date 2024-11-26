using CommunityToolkit.Mvvm.ComponentModel;

namespace PasswordManager.Interfaces
{
    public interface IDataContextProviderService
    {
        ObservableObject ProvideDataContext<TDataContext>() where TDataContext : ObservableObject;
    }
}
