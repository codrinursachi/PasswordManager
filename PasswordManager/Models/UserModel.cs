using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public SecureString Password { get; set; }
    }
}
