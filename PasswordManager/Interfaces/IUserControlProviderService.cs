using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PasswordManager.Interfaces
{
    public interface IUserControlProviderService
    {
        UserControl ProvideUserControl<TUserControl>() where TUserControl : UserControl;
    }
}
