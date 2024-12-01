using PasswordManager.CustomControls;
using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using System.Windows.Controls;
using System.Windows.Data;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for AllPasswordsView.xaml
    /// </summary>
    public partial class AllPasswordsView : UserControl
    {
        public AllPasswordsView(
            IUserControlProviderService userControlProviderService, 
            AllPasswordsViewModel allPasswordsViewModel)
        {
            InitializeComponent();
            DataContext = allPasswordsViewModel;
            var search = userControlProviderService.ProvideUserControl<PasswordSearch>();
            pwdSearch.Content = search;
            Binding searchBind = new("SearchFilter")
            {
                Source = DataContext,
                Mode = BindingMode.OneWayToSource
            };
            search.SetBinding(PasswordSearch.searchCriteriaProperty, searchBind);
            var dataGrid = userControlProviderService.ProvideUserControl<PasswordDataGrid>();
            pwdDataGrid.Content = dataGrid;
        }
    }
}
