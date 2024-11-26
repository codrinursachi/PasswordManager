using PasswordManager.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PasswordManager.CustomControls
{
    /// <summary>
    /// Interaction logic for PasswordTextBox.xaml
    /// </summary>
    public partial class PasswordTextBox : UserControl
    {
        private bool unedited = true;
        public PasswordTextBox()
        {
            InitializeComponent();
        }


        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            int position = ((TextBox)sender).CaretIndex - 1;
            var password = ((IPasswordPair)DataContext).PasswordAsCharArray;
            if ((e.Key == Key.Back || e.Key == Key.Delete) && !string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                ((IPasswordPair)DataContext).PasswordAsCharArray = [.. password[..(position + 1)], .. password[(position + 2)..]];
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                Array.Fill(((IPasswordPair)DataContext).PasswordAsCharArray, '0');
                ((IPasswordPair)DataContext).PasswordAsCharArray = [];
            }
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

        private void pass_GotFocus(object sender, RoutedEventArgs e)
        {
            if (unedited)
            {
                ((IPasswordPair)DataContext).Password = string.Empty;
                ((IPasswordPair)DataContext).PasswordAsCharArray = [];
                unedited = false;
            }
        }
    }
}
