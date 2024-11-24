﻿using PasswordManager.DTO;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.ViewModels;
using PasswordManager.ViewModels.CustomControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PasswordManager.CustomControls
{
    /// <summary>
    /// Interaction logic for PasswordModelEditor.xaml
    /// </summary>
    public partial class PasswordModelEditor : UserControl
    {
        public PasswordModelEditor(
            IDataContextProviderService dataContextProviderService)
        {
            InitializeComponent();
            DataContext=dataContextProviderService.ProvideDataContext<PasswordModelEditorViewModel>();
        }

        private void datePickerGotMouseCapture(object sender, MouseEventArgs e)
        {
            datePicker.SelectedDate = DateTime.Today;
            datePicker.Foreground = Brushes.Black;
        }

        private void datePickerKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (string.IsNullOrEmpty(datePicker.Text))
                {
                    datePicker.SelectedDate = DateTime.Today;
                    datePicker.Foreground = Brushes.Transparent;
                }
            }
        }
    }
}
