using PasswordManager.Models;
using PasswordManager.ViewModels;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            this.MouseMove += ((MainViewModel)this.DataContext).OnActivity;
            this.KeyDown += ((MainViewModel)this.DataContext).OnActivity;
        }

        private void dtbSelectorMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((MainViewModel)this.DataContext).GetDatabases();
            CollectionViewSource.GetDefaultView(dtbSelector)?.Refresh();
            App.Current.Properties["ShouldRefresh"] = true;
        }


        private void CategoriesSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var viewModel = (MainViewModel)DataContext;
            viewModel.Filter = (CategoryNodeModel)e.NewValue;
        }

        private void Categories_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (!(((MainViewModel)DataContext).CurrentChildView is CategoryViewModel))
            {
                categoryRadio.IsChecked = true;
                ((MainViewModel)DataContext).ShowCategoryViewCommand.Execute(null);
                ((TreeViewItem)Categories.ItemContainerGenerator.ContainerFromIndex(0)).IsExpanded=true;
            }
        }
    }
}