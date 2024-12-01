using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
