using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using PasswordManager.ViewModels.Dialogs;
using PasswordManager.Views.Dialogs;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for PasswordDataGrid.xaml
    /// </summary>
    public partial class PasswordDataGrid : UserControl
    {
        public PasswordDataGrid(
            IUserControlProviderService userControlProviderService,
            PasswordDataGridViewModel passwordDataGridViewModel,
            IPasswordManagementService passwordManagementService)
        {
            InitializeComponent();
            DataContext = passwordDataGridViewModel;
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
