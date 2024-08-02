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
    /// Interaction logic for PasswordCreationView.xaml
    /// </summary>
    public partial class PasswordCreationView : Window
    {
        private static bool isLoaded = false;
        public PasswordCreationView()
        {
            InitializeComponent();
        }

        public void Load()
        {
            if (!isLoaded)
            {
                Show();
                isLoaded = true;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            isLoaded = false;
            Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
