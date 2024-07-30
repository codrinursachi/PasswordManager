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
        void Edit(PasswordModel passwordModel, string encryptionData);
        void Remove(PasswordModel passwordModel, string encryptionData);
        ObservableCollection<PasswordModel> GetAllPasswords(string encryptionData);
    }
}
