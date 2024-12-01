using PasswordManager.Interfaces;

namespace PasswordManager.Services
{
    public class PasswordDeletionService : IPasswordDeletionService
    {
        private IPasswordManagementService passwordManagementService;

        public PasswordDeletionService(IPasswordManagementService passwordManagementService)
        {
            this.passwordManagementService = passwordManagementService;
        }

        public int Id { get; set; }

        public void Delete()
        {
            passwordManagementService.Remove(Id);
        }
    }
}
