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
        public AllPasswordsView(IUserControlProviderService userControlProviderService, IDataContextProviderService dataContextProviderService)
        {
            InitializeComponent();
            DataContext = dataContextProviderService.ProvideDataContext<AllPasswordsViewModel>();
            var search = userControlProviderService.ProvideUserControl<PasswordSearch>();
            pwdSearch.Content = search;
            Binding searchBind = new("SearchFilter")
            {
                Source = DataContext,
                Mode = BindingMode.TwoWay
            };
            search.SetBinding(PasswordSearch.searchCriteriaProperty, searchBind);
            var dataGrid = userControlProviderService.ProvideUserControl<PasswordDataGrid>();
            pwdDataGrid.Content = dataGrid;
        }
    }
}
