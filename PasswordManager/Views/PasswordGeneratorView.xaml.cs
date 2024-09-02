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
            DialogResult = true;
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

        private void alphaNumDec_Click(object sender, RoutedEventArgs e)
        {
            if (txtAlphaNums.Text == "0")
            {
                return;
            }
            txtAlphaNums.Text = (int.Parse(txtAlphaNums.Text) - 1).ToString();
        }

        private void alphaNumInc_Click(object sender, RoutedEventArgs e)
        {
            txtAlphaNums.Text = (int.Parse(txtAlphaNums.Text) + 1).ToString();
        }

        private void symNumInc_Click(object sender, RoutedEventArgs e)
        {
            txtSymbols.Text = (int.Parse(txtSymbols.Text) + 1).ToString();
        }

        private void symNumDec_Click(object sender, RoutedEventArgs e)
        {
            if (txtSymbols.Text == "0")
            {
                return;
            }
            txtSymbols.Text = (int.Parse(txtSymbols.Text) - 1).ToString();
        }
    }
}
