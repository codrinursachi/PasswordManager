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
        public static string GetPasswordClearTextById(int id)
        {
            PasswordRepository passwordRepository = new(App.Current.Properties["SelectedDb"].ToString(),App.Current.Properties["pass"].ToString());
            return passwordRepository.GetPasswordById(id).Password;
        }
    }
}
