using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class DatabaseInfoProviderService : IDatabaseInfoProviderService
    {
        public string CurrentDatabase { get; set; }
        public byte[] DBPass { get; set; }
    }
}
