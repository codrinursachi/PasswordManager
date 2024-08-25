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
        void Add(PasswordModel passwordModel, string encryptionData, string databaseName);
        void Edit(int id, PasswordModel newPasswordModel, string encryptionData, string databaseName);
        void Remove(int id, string encryptionData, string databaseName);
        PasswordModel GetPasswordById(int id, string encryptionData, string databaseName);
        List<PasswordModel> GetAllPasswords(string encryptionData,string databaseName);
    }
}
