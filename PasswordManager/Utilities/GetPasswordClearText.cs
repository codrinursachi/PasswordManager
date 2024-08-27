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
            IPasswordRepository passwordRepository = new PasswordRepository();
            return passwordRepository.GetPasswordById(id, App.Current.Properties["pass"].ToString(), App.Current.Properties["SelectedDb"].ToString()+".json").Password;
        }
    }
}
