using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PasswordManager.Interfaces
{
    public interface IDataContextProviderService
    {
        ObservableObject ProvideDataContext<TDataContext>() where TDataContext : ObservableObject;
    }
}
