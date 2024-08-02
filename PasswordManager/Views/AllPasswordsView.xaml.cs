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
        public AllPasswordsView()
        {
            InitializeComponent();
        }

        private void cpyClipboard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetDataObject(((PasswordModel)allPwd.SelectedItem).Password);
        }
    }
}
