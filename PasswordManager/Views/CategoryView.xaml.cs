using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using System.Windows.Controls;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for DirectoryView.xaml
    /// </summary>
    public partial class CategoryView : UserControl
    {
        public CategoryView(
            IUserControlProviderService userControlProviderService)
        {
            InitializeComponent();
            var dataGrid = userControlProviderService.ProvideUserControl<PasswordDataGrid>();
            pwdDataGrid.Content = dataGrid;
        }
    }
}
