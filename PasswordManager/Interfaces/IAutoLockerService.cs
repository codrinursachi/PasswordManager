using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Interfaces
{
    public interface IAutoLockerService
    {
        void OnActivity(object? sender, EventArgs e);
        void SetupTimer();
    }
}
