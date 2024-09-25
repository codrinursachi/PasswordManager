using PasswordManager.DTO;
using PasswordManager.Models;
using PasswordManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Utilities
{
    static class EditPassword
    {
        static public void EditPasswordById(PasswordModel pass, string database, byte[] dBPass)
        {
            PasswordRepository passwordRepository = new(database, dBPass);
            passwordRepository.Edit(pass.Id, pass);
        }
    }
}
