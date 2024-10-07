﻿using PasswordManager.DTO;
using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordManager.ViewModels.CustomControls
{
    class PasswordDataGridViewModel : PasswordCreationViewModel
    {
        PasswordModel passwordModel;
        public PasswordDataGridViewModel() : base()
        {
            AddButtonVisible = false;
        }
        public PasswordModel PasswordModel
        {
            get => passwordModel;
            set
            {
                passwordModel = value;
                Array.Clear(PasswordAsCharArray);
                Id = passwordModel.Id;
                Url = passwordModel.Url;
                Username = passwordModel.Username;
                PasswordAsCharArray = passwordModel.Password;
                ExpirationDate = passwordModel.ExpirationDate;
                CategoryPath = passwordModel.CategoryPath;
                Favorite = passwordModel.Favorite;
                Notes = passwordModel.Notes;
                InitialDatabase = Database;
                CompletedTags.Clear();
                if (string.IsNullOrEmpty(passwordModel.Tags))
                {
                    return;
                }
                foreach (var tag in passwordModel.Tags.Split())
                {
                    CompletedTags.Add(tag);
                }
            }
        }
    }
}
