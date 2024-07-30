using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Repositories
{
    public class PasswordRepository : IPasswordRepository
    {
        readonly string fileName = Thread.CurrentPrincipal.Identity.Name+"EncryptedPass.db";
        public void Add(PasswordModel passwordModel, string encryptionData)
        {
            throw new NotImplementedException();
        }

        public void Edit(PasswordModel currentPasswordModel, PasswordModel newPasswordModel, string encryptionData)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<PasswordModel> GetAllPasswords(string encryptionData)
        {
            throw new NotImplementedException();
        }

        public void Remove(PasswordModel passwordModel, string encryptionData)
        {
            throw new NotImplementedException();
        }
    }
}
