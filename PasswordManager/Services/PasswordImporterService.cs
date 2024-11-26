using Microsoft.Win32;
using PasswordManager.DTO;
using PasswordManager.DTO.Extensions;
using PasswordManager.Interfaces;
using System.IO;
using System.Text.Json;

namespace PasswordManager.Services
{
    public class PasswordImporterService : IPasswordImporterService
    {
        private IDatabaseInfoProviderService databaseInfoProviderService;
        private IPasswordManagementService passwordManagementService;
        public PasswordImporterService(IDatabaseInfoProviderService databaseInfoProviderService, IPasswordManagementService passwordManagementService)
        {
            this.databaseInfoProviderService = databaseInfoProviderService;
            this.passwordManagementService = passwordManagementService;
        }
        public void StartPasswordImport()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Json files(*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                string passwords = File.ReadAllText(openFileDialog.FileName);
                if (string.IsNullOrEmpty(passwords))
                {
                    return;
                }

                List<PasswordImportDTO> passwordsToImport = JsonSerializer.Deserialize<List<PasswordImportDTO>>(passwords);
                foreach (var password in passwordsToImport)
                {
                    passwordManagementService.Add(password.ToPasswordModel());
                }
            }

        }
    }
}
