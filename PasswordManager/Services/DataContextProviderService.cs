using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
