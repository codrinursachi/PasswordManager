﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using PasswordManager.CustomControls;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PasswordManager.ViewModels
{
    partial class LoginViewModel : ObservableValidator, IPasswordPair
    {
        [ObservableProperty]
        private string errorMessage;
        [ObservableProperty]
        private bool isViewVisible = true;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginUserCommand))]
        private string password;
        [ObservableProperty]
        private string buttonText;
        private IDatabaseInfoProviderService databaseInfoProviderService;
        private IUserRepository userRepository;
        public LoginViewModel(
            IDatabaseInfoProviderService databaseInfoProviderService,
            IPathProviderService pathProviderService,
            IUserRepository userRepository)
        {
            this.databaseInfoProviderService = databaseInfoProviderService;
            var path = Path.Combine(pathProviderService.ProgramPath, "UserLogin");
            if (!File.Exists(path))
            {
                ButtonText = "Register";
            }
            else
            {
                ButtonText = "Log in";
            }

            this.userRepository = userRepository;
        }

        public char[] PasswordAsCharArray { get; set; } = [];
       
        private bool CanLogin()
        {
            bool validData;
            if (PasswordAsCharArray.Length < 3)
            {
                validData = false;
            }
            else
            {
                validData = true;
            }

            return validData;
        }

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private void LoginUser()
        {
            var isValidUser = userRepository.AuthenticateUser(PasswordAsCharArray);
            if (isValidUser)
            {
                databaseInfoProviderService.DBPass = ProtectedData.Protect(Encoding.UTF8.GetBytes(PasswordAsCharArray), null, DataProtectionScope.CurrentUser);
                Array.Fill(PasswordAsCharArray, '0');
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "Invalid password";
            }
        }
    }
}
