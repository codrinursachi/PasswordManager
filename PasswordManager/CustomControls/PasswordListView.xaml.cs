using PasswordManager.DTO;
using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for PasswordListView.xaml
    /// </summary>
    public partial class PasswordListView : UserControl
    {
        public static readonly DependencyProperty PasswordListProperty = DependencyProperty.Register("PasswordList", typeof(ObservableCollection<PasswordToShowDTO>), typeof(PasswordListView));
        public PasswordListView()
        {
            InitializeComponent();
        }

        public ObservableCollection<PasswordToShowDTO> PasswordList
        {
            get { return (ObservableCollection<PasswordToShowDTO>)GetValue(PasswordListProperty); }
            set { SetValue(PasswordListProperty, value); }
        }

        private void cpyClipboard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetDataObject(((PasswordToShowDTO)pwdList.SelectedItem).Password);
        }
    }
}
