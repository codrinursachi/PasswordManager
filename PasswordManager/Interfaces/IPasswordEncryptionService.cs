using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Interfaces
{
    public interface IPasswordEncryptionService
    {
        char[] Encrypt(char[] input);
        char[] Decrypt(char[] input);
    }
}
