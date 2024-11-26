using PasswordManager.Models;

namespace PasswordManager.Interfaces
{
    public interface IPasswordRepository
    {
        public void Add(PasswordModel passwordModel);
        public void Edit(int id, PasswordModel newPasswordModel);
        List<PasswordModel> GetAllPasswords();
        PasswordModel GetPasswordById(int id);
        public void Remove(int id);
    }
}
