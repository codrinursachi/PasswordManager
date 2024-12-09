using PasswordManager.CustomControls;
using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using System.Windows;
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
            IUserControlProviderService userControlProviderService)
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            var search = userControlProviderService.ProvideUserControl<PasswordSearch>();
            pwdSearch.Content = search;
            var dataGrid = userControlProviderService.ProvideUserControl<PasswordDataGrid>();
            pwdDataGrid.Content = dataGrid;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
            {
                var search = (PasswordSearch)pwdSearch.Content;
                Binding searchBind = new("SearchFilter")
                {
                    Source = DataContext,
                    Mode = BindingMode.OneWayToSource
                };
                search.SetBinding(PasswordSearch.searchCriteriaProperty, searchBind);
            }
        }
    }
}
