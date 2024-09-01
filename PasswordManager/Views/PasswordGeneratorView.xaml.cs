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
using System.Windows.Shapes;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for PasswordGeneratorView.xaml
    /// </summary>
    public partial class PasswordGeneratorView : Window
    {
        public PasswordGeneratorView()
        {
            InitializeComponent();
            ((PasswordGeneratorViewModel)DataContext).CloseAction = Unload;
        }

        private void Unload()
        {
            DialogResult=true;
            Close();
        }

        private void btnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void pnlControlBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
