using System.Windows;
using System.Windows.Controls;

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
