using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Interfaces
{
    interface IPasswordSettable
    {
        string Password { get; set; }
    }
}
