using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Models
{
    interface IPasswordRepository
    {
        void Add(PasswordModel passwordModel, string encryptionData);
        void Edit(int id, PasswordModel newPasswordModel, string encryptionData);
        void Remove(int id, string encryptionData);
        PasswordModel GetPasswordById(int id, string encryptionData);
        List<PasswordModel> GetAllPasswords(string encryptionData);
    }
}
