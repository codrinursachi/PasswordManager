using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Views;
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
    partial class PasswordGeneratorViewModel : ObservableObject
    {
        private string alphaNum = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private string symbols = "!\\\";#$%&'()*+,-./:;<=>?@[]^`{|}~";
        public int AlphaNumCount { get; set; } = 5;
        public int SymbolsCount { get; set; } = 5;

        public char[] GeneratedPassword { get; set; } = [];

        [RelayCommand]
        private void AcceptPassword(object obj)
        {
            foreach(Window window in App.Current.Windows)
            {
                if (window is PasswordGeneratorView)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
        }

        [RelayCommand]
        private void GeneratePassword(object obj)
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
