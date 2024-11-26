using PasswordManager.Interfaces;

namespace PasswordManager.Services
{
    public class DatabaseInfoProviderService : IDatabaseInfoProviderService
    {
        public string CurrentDatabase { get; set; }
        public byte[] DBPass { get; set; }
    }
}
