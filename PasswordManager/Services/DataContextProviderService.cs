using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;

namespace PasswordManager.Services
{
    public class DataContextProviderService : IDataContextProviderService
    {
        private Func<Type, ObservableObject> dataContextFactory;

        public DataContextProviderService(Func<Type, ObservableObject> dataContextFactory)
        {
            this.dataContextFactory = dataContextFactory;
        }
        public ObservableObject ProvideDataContext<TDataContext>() where TDataContext : ObservableObject
        {
            return dataContextFactory.Invoke(typeof(TDataContext));
        }
    }
}
