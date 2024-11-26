using PasswordManager.Interfaces;
using PasswordManager.ViewModels.CustomControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PasswordManager.CustomControls
{
    /// <summary>
    /// Interaction logic for PasswordModelEditor.xaml
    /// </summary>
    public partial class PasswordModelEditor : UserControl
    {
        public PasswordModelEditor(
            IDataContextProviderService dataContextProviderService)
        {
            InitializeComponent();
            DataContext = dataContextProviderService.ProvideDataContext<PasswordModelEditorViewModel>();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            int position = ((TextBox)sender).CaretIndex - 1;
            var password = ((PasswordModelEditorViewModel)DataContext).PasswordAsCharArray;
            if ((e.Key == Key.Back || e.Key == Key.Delete) && !string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                ((PasswordModelEditorViewModel)DataContext).PasswordAsCharArray = [.. password[..(position + 1)], .. password[(position + 2)..]];
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                Array.Fill(((PasswordModelEditorViewModel)DataContext).PasswordAsCharArray, '0');
                ((PasswordModelEditorViewModel)DataContext).PasswordAsCharArray = [];
            }
            if (e.IsRepeat)
            {
                e.Handled = true;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int position = ((TextBox)sender).CaretIndex;
            var password = ((PasswordModelEditorViewModel)DataContext).PasswordAsCharArray;
            var passwordChar = e.Text[0];
            if (char.IsControl(passwordChar))
            {
                return;
            }

            ((PasswordModelEditorViewModel)DataContext).PasswordAsCharArray = [.. password[..position], passwordChar, .. password[position..]];
            var length = ((PasswordModelEditorViewModel)DataContext).PasswordAsCharArray.Length;
            ((PasswordModelEditorViewModel)DataContext).Password = string.Concat(Enumerable.Repeat('*', length));
            ((TextBox)sender).CaretIndex = position + 1;
            e.Handled = true;
        }

        private void pass_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((PasswordModelEditorViewModel)DataContext).UneditedPass && !((PasswordModelEditorViewModel)DataContext).IsReadOnly)
            {
                ((PasswordModelEditorViewModel)DataContext).Password = string.Empty;
                ((PasswordModelEditorViewModel)DataContext).PasswordAsCharArray = [];
                ((PasswordModelEditorViewModel)DataContext).UneditedPass = false;
            }
        }
    }
}
