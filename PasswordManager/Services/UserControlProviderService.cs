using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PasswordManager.Services
{
    public class UserControlProviderService : IUserControlProviderService
    {
        private Func<Type, UserControl> userControlFactory;

        public UserControlProviderService(Func<Type, UserControl> userControlFactory)
        {
            this.userControlFactory = userControlFactory;
        }

        public UserControl ProvideUserControl<TUserControl>() where TUserControl : UserControl
        {
            UserControl userControl = userControlFactory.Invoke(typeof(TUserControl));
            return userControl;
        }
    }
}
