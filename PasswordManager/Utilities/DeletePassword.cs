using PasswordManager.Repositories;

namespace PasswordManager.Utilities
{
    static class DeletePassword
    {
        public static void DeletePasswordById(int id, string database, byte[] dBPass)
        {
            PasswordRepository passwordRepository = new(database, dBPass);
            passwordRepository.Remove(id);
        }
    }
}
