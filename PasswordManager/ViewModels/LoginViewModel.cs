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
        public ICommand RegisterCommand { get; }
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isViewVisible = true;
        private UserRepository _userRepository;

        public LoginViewModel()
        {
            _userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteOperationCommand);
            RegisterCommand = new ViewModelCommand(ExecuteRegisterCommand, CanExecuteOperationCommand);
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool IsViewVisible
        {
            get => _isViewVisible;
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        private bool CanExecuteOperationCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrEmpty(Username) || Username.Length < 3 || Password == null || Password.Length < 3)
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
            var isValidUser = _userRepository.AuthenticateUser(new NetworkCredential(Username, Password));
            if (isValidUser)
            {
                App.Current.Properties["pass"] = Password;
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "Invalid username or password";
            }
        }

        private void ExecuteRegisterCommand(object obj)
        {
            _userRepository.Add(new UserModel { UserName = Username, Password = Password });
            var isValidUser = _userRepository.AuthenticateUser(new NetworkCredential(Username, Password));
            if (isValidUser)
            {
                App.Current.Properties["pass"] = Password;
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                Directory.CreateDirectory(Thread.CurrentPrincipal.Identity.Name);
                Directory.CreateDirectory(Thread.CurrentPrincipal.Identity.Name+"\\Backups");
                File.Create(Thread.CurrentPrincipal.Identity.Name+"\\" + "default").Close();
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "Username already taken";
            }
        }
    }
}
