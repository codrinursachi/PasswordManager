using Microsoft.Win32;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Models.Extensions;
using System.IO;
using System.Text.Json;

namespace PasswordManager.Services
{
    public class PasswordImporterService : IPasswordImporterService
    {
        private IPasswordManagementService passwordManagementService;

        public PasswordImporterService(
            IPasswordManagementService passwordManagementService)
        {
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

                List<PasswordImportModel> passwordsToImport = JsonSerializer.Deserialize<List<PasswordImportModel>>(passwords);
                foreach (var password in passwordsToImport)
                {
                    passwordManagementService.Add(password.ToPasswordModel());
                }
            }
        }
    }
}
