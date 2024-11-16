using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PasswordManager.ViewModels
{
    class PasswordGeneratorViewModel : ViewModelBase
    {
        private string alphaNum = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private string symbols = "!\\\";#$%&'()*+,-./:;<=>?@[]^`{|}~";
        public PasswordGeneratorViewModel()
        {
            GeneratePasswordCommand = new ViewModelCommand(ExecuteGeneratePasswordCommand);
            AcceptPasswordCommand = new ViewModelCommand(ExecuteAcceptPasswordCommand);
        }
        public int AlphaNumCount { get; set; } = 5;
        public int SymbolsCount { get; set; } = 5;
        public Action CloseAction { get; set; }

        public char[] GeneratedPassword { get; set; } = [];
        public ICommand GeneratePasswordCommand { get; }
        public ICommand AcceptPasswordCommand { get; }

        private void ExecuteAcceptPasswordCommand(object obj)
        {
            CloseAction.Invoke();
        }

        private void ExecuteGeneratePasswordCommand(object obj)
        {
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
    }
}
