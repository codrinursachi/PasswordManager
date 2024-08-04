using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DTO
{
    public class PasswordToShowDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string ExpirationDate { get; set; } = string.Empty;
        public string CategoryPath { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public bool Favorite { get; set; } = false;
        public string Database { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
