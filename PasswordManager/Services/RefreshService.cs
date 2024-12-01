using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class RefreshService : IRefreshService
    {
        public IRefreshable Main { get; set; }
        public IRefreshable View { get; set; }

        public void RefreshMain()
        {
            Main.Refresh();
        }

        public void RefreshPasswords()
        {
            View.Refresh();
        }
    }
}
