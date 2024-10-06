using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
    /// Interaction logic for PasswordTextBox.xaml
    /// </summary>
    public partial class PasswordTextBox : UserControl
    {
        public PasswordTextBox()
        {
            InitializeComponent();
        }


        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            int position = ((TextBox)sender).CaretIndex - 1;
            var password = ((IPasswordPair)DataContext).PasswordAsCharArray;
            if ((e.Key == Key.Back || e.Key == Key.Delete) && ((TextBox)sender).Text != null)
            {
                ((IPasswordPair)DataContext).PasswordAsCharArray = [.. password[..(position + 1)], .. password[(position + 2)..]];
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat)
            {
                e.Handled = true;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int position = ((TextBox)sender).CaretIndex;
            var password = ((IPasswordPair)DataContext).PasswordAsCharArray;
            var passwordChar = e.Text[0];
            if (char.IsControl(passwordChar))
            {
                return;
            }
            ((IPasswordPair)DataContext).PasswordAsCharArray = [.. password[..position], passwordChar, .. password[position..]];
            var length = ((IPasswordPair)DataContext).PasswordAsCharArray.Length;
            ((IPasswordPair)DataContext).Password = string.Concat(Enumerable.Repeat('*', length));
            ((TextBox)sender).CaretIndex = position + 1;
            e.Handled = true;
        }
    }
}
