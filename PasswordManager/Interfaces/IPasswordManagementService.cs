using PasswordManager.Models;

namespace PasswordManager.Interfaces
{
    public interface IPasswordManagementService
    {
        CategoryNodeModel GetPasswordsCategoryRoot();
        void Add(PasswordModel passwordModel);
        void Edit(int id, PasswordModel newPasswordModel);
        List<PasswordToShowModel> GetAllPasswords();
        PasswordModel GetPasswordById(int id);
        void Remove(int id);
        List<PasswordToShowModel> GetFilteredPasswords(string filter);
    }
}
