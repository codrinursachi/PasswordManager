﻿using Moq;
using PasswordManager;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.Services;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace PasswordManagerTests.ViewModels
{
    public class AllPasswordsViewModelTests
    {
        [Fact]
        public void ShouldStorePasswords()
        {
            string file = Path.Combine("Temporary",Path.GetRandomFileName());
            string pathToTemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", "Temporary");
            if (!Directory.Exists(pathToTemp))
            {
                Directory.CreateDirectory(pathToTemp);
            }
            DatabaseInfoProviderService databaseInfoProviderService = new()
            {
                CurrentDatabase = file,
                DBPass = ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser)
            };
            PasswordRepository passwordRepository = new(databaseInfoProviderService);
            PasswordManagementService passwordManagementService = new(passwordRepository);
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com" };
            PasswordModel password2 = new() { Username = "admin2", Password = "admin2".ToCharArray(), Url = "admin2.com" };
            PasswordModel password3 = new() { Username = "admin3", Password = "admin3".ToCharArray(), Url = "admin3.com" };
            passwordRepository.Add(password);
            passwordRepository.Add(password2);
            passwordRepository.Add(password3);
            AllPasswordsViewModel allPasswordsViewModel = new(databaseInfoProviderService,passwordManagementService);
            allPasswordsViewModel.Refresh();
            Assert.Equal(3, allPasswordsViewModel.Passwords.Count);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file + ".json"));
        }

        [Fact]
        public void ShouldFilterPasswords()
        {
            string file = Path.Combine("Temporary", Path.GetRandomFileName());
            string pathToTemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", "Temporary");
            if (!Directory.Exists(pathToTemp))
            {
                Directory.CreateDirectory(pathToTemp);
            }
            DatabaseInfoProviderService databaseInfoProviderService = new()
            {
                CurrentDatabase = file,
                DBPass = ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser)
            };
            PasswordRepository passwordRepository = new(databaseInfoProviderService);
            PasswordManagementService passwordManagementService = new(passwordRepository);
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com" };
            PasswordModel password2 = new() { Username = "admin2", Password = "admin2".ToCharArray(), Url = "admin2.com" };
            PasswordModel password3 = new() { Username = "admin3", Password = "admin3".ToCharArray(), Url = "admin3.com" };
            passwordRepository.Add(password);
            passwordRepository.Add(password2);
            passwordRepository.Add(password3);
            AllPasswordsViewModel allPasswordsViewModel = new(databaseInfoProviderService, passwordManagementService);
            allPasswordsViewModel.SearchFilter = "admin2";
            Assert.Single(allPasswordsViewModel.Passwords);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file + ".json"));
        }
    }
}
