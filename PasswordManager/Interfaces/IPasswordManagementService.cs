using PasswordManager.DTO;
using PasswordManager.Models;

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
