using PasswordManager.CustomControls;
using PasswordManager.Interfaces;
using PasswordManager.ViewModels.Dialogs;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PasswordManager.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for PasswordModelEditor.xaml
    /// </summary>
    public partial class PasswordModelEditor : UserControl
    {
        public PasswordModelEditor(
            IUserControlProviderService userControlProviderService)
        {
            InitializeComponent();
            passGenOverlay.Content = userControlProviderService.ProvideUserControl<DialogOverlay>(); 
            DataContextChanged += OnDataContextChanged;
            pwdTxtBox.Content = userControlProviderService.ProvideUserControl<PasswordTextBox>();
        }


        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
            {
                var passwordTextBox = (PasswordTextBox)pwdTxtBox.Content;
                Binding pwdCharArrayBind = new("PasswordAsCharArray")
                {
                    Source = DataContext,
                    Mode = BindingMode.TwoWay
                };

                Binding pwdMaskBind = new("Password")
                {
                    Source = DataContext,
                    Mode = BindingMode.TwoWay
                };

                Binding isReadOnlyBind=new("IsReadOnly")
                {
                    Source = DataContext,
                    Mode = BindingMode.OneWay
                };

                passwordTextBox.SetBinding(PasswordTextBox.PasswordAsCharArrayProperty, pwdCharArrayBind);
                passwordTextBox.SetBinding(PasswordTextBox.PasswordProperty, pwdMaskBind);
                passwordTextBox.SetBinding(PasswordTextBox.ReadOnlyProperty, isReadOnlyBind);
            }
        }
    }
}
