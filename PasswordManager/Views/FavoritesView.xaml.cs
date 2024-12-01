using PasswordManager.CustomControls;
using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using System.Windows.Controls;
using System.Windows.Data;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for FavoritesView.xaml
    /// </summary>
    public partial class FavoritesView : UserControl
    {
        public FavoritesView(
            IUserControlProviderService userControlProviderService,
            FavoritesViewModel favoritesViewModel)
        {
            InitializeComponent();
            DataContext = favoritesViewModel;
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
