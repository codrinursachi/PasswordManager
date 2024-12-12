using PasswordManager.Interfaces;

namespace PasswordManager.State
{
    public class DatabaseState
    {
        public string CurrentDatabase { get; set; }
        public byte[] DBPass { get; set; }
    }
}
