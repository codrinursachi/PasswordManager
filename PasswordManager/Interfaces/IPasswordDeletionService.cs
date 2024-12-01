using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Interfaces
{
    public interface IPasswordDeletionService
    {
        int Id { get; set; }
        void Delete();
    }
}
