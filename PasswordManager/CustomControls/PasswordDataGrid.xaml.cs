﻿using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.DTO;
using PasswordManager.DTO.Extensions;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Utilities;
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
using System.Windows.Threading;

namespace PasswordManager.CustomControls
{
    /// <summary>
    /// Interaction logic for PasswordListView.xaml
    /// </summary>
    public partial class PasswordDataGrid : UserControl
    {
        public static readonly DependencyProperty PasswordListProperty = DependencyProperty.Register("PasswordList", typeof(ObservableCollection<PasswordToShowDTO>), typeof(PasswordDataGrid));

        private char[] storedPass;
        private DispatcherTimer timer;

        private IPasswordManagementService passwordManagementService;
        private IMessenger passwordModelMessenger;
        public PasswordDataGrid(
            IUserControlProviderService userControlProviderService, 
            IPasswordManagementService passwordManagementService,
            [FromKeyedServices("PasswordModel")] IMessenger passwordModelMessenger)
        {
            InitializeComponent();
            var passwordModelEditor = userControlProviderService.ProvideUserControl<PasswordModelEditor>();
            ((PasswordModelEditorViewModel)passwordModelEditor.DataContext).AddButtonVisible = false;
            pwdEditor.Content = passwordModelEditor;
            this.passwordManagementService = passwordManagementService;
            this.passwordModelMessenger = passwordModelMessenger;
        }

        public ObservableCollection<PasswordToShowDTO> PasswordList
        {
            get { return (ObservableCollection<PasswordToShowDTO>)GetValue(PasswordListProperty); }
            set { SetValue(PasswordListProperty, value); }
        }

        private void SetupTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (Clipboard.GetText().ToCharArray() == storedPass)
            {
                Clipboard.Clear();
            }

            Array.Clear(storedPass);
            timer.Stop();
        }

        private void pwdList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (PasswordToShowDTO)pwdList.SelectedItem;
            if (selectedItem == null)
            {
                return;
            }
            passwordModelMessenger.Send(selectedItem.ToPasswordModel());
        }

        private void MenuItemUsername_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetDataObject(((PasswordToShowDTO)pwdList.SelectedItem).Username);
        }

        private void MenuItemPassword_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            storedPass = passwordManagementService.GetPasswordById(((PasswordToShowDTO)pwdList.SelectedItem).Id).Password;
            Clipboard.SetDataObject(new string(storedPass));
            SetupTimer();
        }

        private void MenuItemDelete_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            passwordManagementService.Remove(((PasswordToShowDTO)pwdList.SelectedItem).Id);
            PasswordList.Remove((PasswordToShowDTO)pwdList.SelectedItem);
        }

        private void pwdList_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (pwdList.SelectedItem == null)
            {
                e.Handled = true;
            }
        }
    }
}
