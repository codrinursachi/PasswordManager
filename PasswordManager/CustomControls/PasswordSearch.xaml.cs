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

namespace PasswordManager.CustomControls
{
    /// <summary>
    /// Interaction logic for PasswordSearch.xaml
    /// </summary>
    public partial class PasswordSearch : UserControl
    {
        public static readonly DependencyProperty searchCriteriaProperty = DependencyProperty.Register("SearchCriteria", typeof(string), typeof(PasswordSearch));
        public PasswordSearch()
        {
            InitializeComponent();
        }

        public string SearchCriteria
        {
            get { return (string)GetValue(searchCriteriaProperty); }
            set { SetValue(searchCriteriaProperty, value); }
        }

        private void searchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchBar.Text))
            {
                placeholder.Visibility = Visibility.Visible;
            }
            else
            {
                placeholder.Visibility = Visibility.Hidden;
            }
        }
    }
}
