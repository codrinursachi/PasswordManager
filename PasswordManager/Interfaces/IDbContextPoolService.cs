using PasswordManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Interfaces
{
    public interface IDbContextPoolService
    {
        PasswordManagerDbContext Acquire(string databaseName);
        void Release(string databaseName, PasswordManagerDbContext context);
    }
}
