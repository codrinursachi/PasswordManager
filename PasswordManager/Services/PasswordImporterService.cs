using Microsoft.Win32;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

                List<PasswordModel> passwordsToImport = JsonSerializer.Deserialize<List<PasswordModel>>(passwords);
                foreach (var password in passwordsToImport)
                {
                    passwordManagementService.Add(password);
                }
            }

        }
    }
}
