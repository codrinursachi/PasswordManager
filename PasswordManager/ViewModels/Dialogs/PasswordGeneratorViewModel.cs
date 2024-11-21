using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PasswordManager.Interfaces;
using PasswordManager.Views;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;

namespace PasswordManager.ViewModels
{
    partial class PasswordGeneratorViewModel : ObservableValidator
    {
        private string alphaNum = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private string symbols = "!\\\";#$%&'()*+,-./:;<=>?@[]^`{|}~";
        [ObservableProperty]
        [Range(0, int.MaxValue,ErrorMessage ="Value cannot be negative")]
        private int alphaNumCount = 5;
        [ObservableProperty]
        [Range(0, int.MaxValue,ErrorMessage ="Value cannot be negative")]
        private int symbolsCount = 5;
        [ObservableProperty]
        private string alphaNumCountErrorMessage;
        [ObservableProperty]
        private string symbolsCountErrorMessage;
        [ObservableProperty]
        private char[] generatedPassword = [];
        private IModalDialogClosingService modalDialogClosingService;
        private IMessenger messenger;

        public PasswordGeneratorViewModel(
            IModalDialogClosingService modalDialogClosingService,
            IMessenger messenger)
        {
            this.modalDialogClosingService = modalDialogClosingService;
            this.messenger = messenger;
        }

        [RelayCommand]
        public void IncrementAlphaNumCount() => AlphaNumCount++;

        [RelayCommand]
        public void IncrementSymbolsCount() => SymbolsCount++;

        [RelayCommand]
        public void DecrementAlphaNumCount() => AlphaNumCount = AlphaNumCount > 0 ? AlphaNumCount - 1 : 0;

        [RelayCommand]
        public void DecrementSymbolsCount() => SymbolsCount = SymbolsCount > 0 ? SymbolsCount - 1 : 0;

        [RelayCommand]
        private void AcceptPassword(object obj)
        {
            messenger.Send(GeneratedPassword);
            modalDialogClosingService.Close();
        }

        [RelayCommand]
        private void GeneratePassword(object obj)
        {
            ValidateAllProperties();
            if (HasErrors)
            {
                SetValidationErrorsStrings();
                return;
            }
            Array.Fill(GeneratedPassword, '0');
            GeneratedPassword = new char[AlphaNumCount + SymbolsCount];
            for (int i = 0; i < AlphaNumCount; i++)
            {
                GeneratedPassword[i] = alphaNum[Random.Shared.Next(alphaNum.Length)];
            }

            for (int i = 0; i < SymbolsCount; i++)
            {
                GeneratedPassword[i + AlphaNumCount] = symbols[Random.Shared.Next(symbols.Length)];
            }

            Random.Shared.Shuffle(GeneratedPassword);
            var textbox = (TextBox)obj;
            textbox.Clear();
            foreach (char c in GeneratedPassword)
            {
                textbox.Text += c;
            }
        }

        private void SetValidationErrorsStrings()
        {
            AlphaNumCountErrorMessage = string.Empty;
            SymbolsCountErrorMessage = string.Empty;
            var errorDict = GetErrors().ToDictionary(e => e.MemberNames.First(), e => e.ErrorMessage);
            errorDict.TryGetValue(nameof(AlphaNumCount), out alphaNumCountErrorMessage);
            errorDict.TryGetValue(nameof(SymbolsCount), out symbolsCountErrorMessage);
            OnPropertyChanged(nameof(AlphaNumCountErrorMessage));
            OnPropertyChanged(nameof(SymbolsCountErrorMessage));
        }
    }
}
