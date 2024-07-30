using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Models
{
    public class PasswordModel
    {
        public string username;
        public string password;
        public string url;
        public string notes;
        public DateTime expirationTime;
        public string categoryPath;
        public string tags;
        public bool favorite;
        public string database;
    }
}
