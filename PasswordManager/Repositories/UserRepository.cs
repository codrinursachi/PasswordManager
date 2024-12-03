using PasswordManager.Interfaces;
using System.IO;

namespace PasswordManager.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly string fileName;
        private ISecretHasherService secretHasherService;

        public UserRepository(
            IPathProviderService pathProviderService,
            ISecretHasherService secretHasherService)
        {
            fileName = Path.Combine(pathProviderService.ProgramPath, "UserLogin");
            this.secretHasherService = secretHasherService;
        }

        private void Add(char[] password)
        {
            var passwordHash = secretHasherService.Hash(password, 50000);
            File.WriteAllText(fileName, passwordHash);
        }

        public bool AuthenticateUser(char[] password)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }
            string passwordHash = File.ReadAllText(fileName);
            if (string.IsNullOrEmpty(passwordHash))
            {
                Add(password);
                return true;
            }

            return secretHasherService.Verify(password, passwordHash);
        }
    }
}
