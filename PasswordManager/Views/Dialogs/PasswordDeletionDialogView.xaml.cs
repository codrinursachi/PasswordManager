﻿using PasswordManager.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManager.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for PasswordDeletionDialogView.xaml
    /// </summary>
    public partial class PasswordDeletionDialogView : UserControl
    {
        public PasswordDeletionDialogView(
            PasswordDeletionDialogViewModel passwordDeletionDialogViewModel)
        {
            InitializeComponent();
            DataContext = passwordDeletionDialogViewModel;
        }
    }
}
