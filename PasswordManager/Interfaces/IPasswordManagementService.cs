using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Interfaces
{
    public interface IPasswordManagementService
    {
        CategoryNodeModel GetPasswordsCategoryRoot();
        public void Add(PasswordModel passwordModel);
        public void Edit(int id, PasswordModel newPasswordModel);
        List<PasswordModel> GetAllPasswords();
        PasswordModel GetPasswordById(int id);
        public void Remove(int id);
    }
}
