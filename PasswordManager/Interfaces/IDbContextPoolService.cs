using PasswordManager.Data;

namespace PasswordManager.Interfaces
{
    public interface IDbContextPoolService
    {
        PasswordManagerDbContext Acquire(string databaseName);
        void Release(string databaseName, PasswordManagerDbContext context);
    }
}
