using PasswordManager.CustomControls;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
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
            var search= userControlProviderService.ProvideUserControl<PasswordSearch>();
            pwdSearch.Content = search;
            Binding searchBind = new("SearchFilter")
            {
                Source = DataContext,
                Mode = BindingMode.TwoWay
            };
            search.SetBinding(PasswordSearch.searchCriteriaProperty, searchBind);
            var dataGrid= userControlProviderService.ProvideUserControl<PasswordDataGrid>();
            pwdDataGrid.Content = dataGrid;
            Binding dataGridBinding = new("Passwords")
            {
                Source = DataContext
            };
            dataGrid.SetBinding(PasswordDataGrid.PasswordListProperty, dataGridBinding);
        }
    }
}
