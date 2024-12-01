using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Interfaces
{
    public interface IRefreshService
    {
        IRefreshable Main { get; set; }
        IRefreshable View { get; set; }
        void RefreshMain();
        void RefreshPasswords();
    }
}
