using Microsoft.Win32;
using PasswordManager.Models;
using PasswordManager.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PasswordManager.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; }
        private string username;
        private string password;
        private string errorMessage;
        private bool isViewVisible = true;
        private UserRepository userRepository;

        public LoginViewModel()
        {
            userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteOperationCommand);
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
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

        private bool CanExecuteOperationCommand(object obj)
        {
            bool validData;
            if (Password == null || Password.Length < 3)
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
            var isValidUser = userRepository.AuthenticateUser(Password);
            if (isValidUser)
            {
                App.Current.Properties["pass"] = Password;
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "Invalid password";
            }
        }
    }
}
