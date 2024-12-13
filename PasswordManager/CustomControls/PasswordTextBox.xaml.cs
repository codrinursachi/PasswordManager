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
        public static readonly DependencyProperty UneditedProperty = DependencyProperty.Register(nameof(Unedited), typeof(bool), typeof(PasswordTextBox));
        public static readonly DependencyProperty PasswordAsCharArrayProperty = DependencyProperty.Register(nameof(PasswordAsCharArray), typeof(char[]), typeof(PasswordTextBox));
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(nameof(Password), typeof(string), typeof(PasswordTextBox));
        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(nameof(ReadOnly), typeof(bool), typeof(PasswordTextBox));

        public PasswordTextBox()
        {
            InitializeComponent();
            Unedited = true;
            DataContext = this;
        }

        public bool Unedited
        {
            get => (bool)GetValue(UneditedProperty);
            set => SetValue(UneditedProperty, value);
        }

        public char[] PasswordAsCharArray
        {
            get => (char[])GetValue(PasswordAsCharArrayProperty);
            set => SetValue(PasswordAsCharArrayProperty, value);
        }

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public bool ReadOnly
        {
            get => (bool)GetValue(ReadOnlyProperty);
            set => SetValue(ReadOnlyProperty, value);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (ReadOnly)
            {
                return;
            }

            int position = ((TextBox)sender).CaretIndex - 1;
            if ((e.Key == Key.Back || e.Key == Key.Delete) && !string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                PasswordAsCharArray = [.. PasswordAsCharArray[..(position + 1)], .. PasswordAsCharArray[(position + 2)..]];
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (ReadOnly)
            {
                return;
            }

            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                Array.Fill(PasswordAsCharArray, '0');
                PasswordAsCharArray = [];
            }
            if (e.IsRepeat)
            {
                e.Handled = true;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (ReadOnly)
            {
                return;
            }

            int position = ((TextBox)sender).CaretIndex;
            var passwordChar = e.Text[0];
            if (char.IsControl(passwordChar))
            {
                return;
            }

            PasswordAsCharArray = [.. PasswordAsCharArray[..position], passwordChar, .. PasswordAsCharArray[position..]];
            var length = PasswordAsCharArray.Length;
            Password = string.Concat(Enumerable.Repeat('*', length));
            ((TextBox)sender).CaretIndex = position + 1;
            e.Handled = true;
        }

        private void pass_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!ReadOnly && Unedited)
            {
                Password = string.Empty;
                PasswordAsCharArray = [];
                Unedited = false;
            }
        }
    }
}
