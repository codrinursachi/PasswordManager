using PasswordManager.Interfaces;
using PasswordManager.ViewModels.CustomControls;
using System.Windows.Controls;
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
