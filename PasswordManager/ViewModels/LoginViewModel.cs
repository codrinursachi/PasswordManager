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
    class LoginViewModel : ViewModelBase, IPasswordSettable, IPasswordPair
    {
        public ICommand LoginCommand { get; }
        private string errorMessage;
        private bool isViewVisible = true;
        private UserRepository userRepository;
        private string password;
        private string buttonText;

        public LoginViewModel()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "UserLogin.json");
            if (!File.Exists(path))
            {
                ButtonText = "Register";
            }
            else
            {
                ButtonText = "Log in";
            }
            userRepository = new UserRepository("UserLogin.json");
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteOperationCommand);
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool IsViewVisible
        {
            get => isViewVisible;
            set
            {
                isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        public byte[] DBPass { get; set; }
        public char[] PasswordAsCharArray { get; set; } = [];
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ButtonText
        {
            get => buttonText;
            set
            {
                buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        private bool CanExecuteOperationCommand(object obj)
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

        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = userRepository.AuthenticateUser(PasswordAsCharArray);
            if (isValidUser)
            {
                DBPass = ProtectedData.Protect(Encoding.UTF8.GetBytes(PasswordAsCharArray), null, DataProtectionScope.CurrentUser);
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
