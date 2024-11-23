using PasswordManager.DTO;
using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Interfaces
{
    public interface IPasswordManagementService
    {
        CategoryNodeModel GetPasswordsCategoryRoot();
        void Add(PasswordModel passwordModel);
        void Edit(int id, PasswordModel newPasswordModel);
        List<PasswordToShowDTO> GetAllPasswords();
        PasswordModel GetPasswordById(int id);
        void Remove(int id);
        List<PasswordToShowDTO> GetFilteredPasswords(string filter);
    }
}
