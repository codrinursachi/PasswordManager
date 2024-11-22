using CommunityToolkit.Mvvm.Messaging;
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
        private char[] storedPass;
        private DispatcherTimer timer;

        private IPasswordManagementService passwordManagementService;
        public PasswordDataGrid(
            IUserControlProviderService userControlProviderService,
            IDataContextProviderService dataContextProviderService,
            IPasswordManagementService passwordManagementService)
        {
            InitializeComponent();
            DataContext = dataContextProviderService.ProvideDataContext<PasswordDataGridViewModel>();
            var passwordModelEditor = userControlProviderService.ProvideUserControl<PasswordModelEditor>();
            ((PasswordModelEditor)passwordModelEditor).EditMode = true;
            pwdEditor.Content = passwordModelEditor;
            this.passwordManagementService = passwordManagementService;
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
