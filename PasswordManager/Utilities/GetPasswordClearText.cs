using PasswordManager.Models;
using PasswordManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Utilities
{
    static class GetPasswordClearText
    {
        public static char[] GetPasswordClearTextById(int id, string database, byte[] dBPass)
        {
            PasswordRepository passwordRepository = new(database, dBPass);
            return passwordRepository.GetPasswordById(id).Password;
        }
    }
}
