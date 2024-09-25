﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PasswordManager.ViewModels
{
    class PasswordGeneratorViewModel : ViewModelBase
    {
        private string alphaNum = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private string symbols = "!\\\";#$%&'()*+,-./:;<=>?@[]^`{|}~";
        private string generatedPassword;
        public PasswordGeneratorViewModel()
        {
            GeneratePasswordCommand = new ViewModelCommand(ExecuteGeneratePasswordCommand);
            AcceptPasswordCommand = new ViewModelCommand(ExecuteAcceptPasswordCommand);
        }
        public int AlphaNumCount { get; set; } = 5;
        public int SymbolsCount { get; set; } = 5;
        public Action CloseAction { get; set; }

        public string GeneratedPassword
        {
            get => generatedPassword;
            set
            {
                generatedPassword = value;
                OnPropertyChanged(nameof(GeneratedPassword));
            }
        }
        public ICommand GeneratePasswordCommand { get; }
        public ICommand AcceptPasswordCommand { get; }

        private void ExecuteAcceptPasswordCommand(object obj)
        {
            Clipboard.SetDataObject(GeneratedPassword);
            CloseAction.Invoke();
        }

        private void ExecuteGeneratePasswordCommand(object obj)
        {
            StringBuilder pass = new();
            for (int i = 0; i < AlphaNumCount; i++)
            {
                pass.Append(alphaNum[Random.Shared.Next(alphaNum.Length)]);
            }

            for (int i = 0; i < SymbolsCount; i++)
            {
                pass.Append(symbols[Random.Shared.Next(symbols.Length)]);
            }

            char[] shuffledArray = pass.ToString().ToCharArray();
            Random.Shared.Shuffle(shuffledArray);
            GeneratedPassword = new(shuffledArray);
        }
    }
}
