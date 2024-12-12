using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Interfaces;
using PasswordManager.State;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.ViewModels
{
    public partial class LoginViewModel : ObservableValidator, IPasswordPair
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
        private DatabaseState databaseInfoProviderService;
        private IUserRepository userRepository;

        public LoginViewModel(
            DatabaseState databaseInfoProviderService,
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
