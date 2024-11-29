using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Interfaces
{
    public interface ISecretHasherService
    {
        string Hash(char[] input, int iterations);
        bool Verify(char[] input, string hashString);
    }
}
