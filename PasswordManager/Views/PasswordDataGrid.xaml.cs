using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using PasswordManager.Views.Dialogs;
using System.Windows.Controls;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for PasswordDataGrid.xaml
    /// </summary>
    public partial class PasswordDataGrid : UserControl
    {
        public PasswordDataGrid(
            IUserControlProviderService userControlProviderService,
            IPasswordManagementService passwordManagementService)
        {
            InitializeComponent();
            var passwordModelEditor = userControlProviderService.ProvideUserControl<PasswordModelEditor>();
            pwdEditor.Content = passwordModelEditor;
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
