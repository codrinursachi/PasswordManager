using PasswordManager.CustomControls;
using PasswordManager.Interfaces;
using PasswordManager.Services;
using PasswordManager.ViewModels;
using System.Windows;
using System.Windows.Data;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView(
            IUserControlProviderService userControlProviderService)
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            pwdTxtBox.Content = userControlProviderService.ProvideUserControl<PasswordTextBox>();
        }


        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
            {
                var passwordTextBox = (PasswordTextBox)pwdTxtBox.Content;
                Binding pwdBind = new("PasswordAsCharArray")
                {
                    Source = DataContext,
                    Mode = BindingMode.OneWayToSource
                };

                passwordTextBox.SetBinding(PasswordTextBox.PasswordAsCharArrayProperty, pwdBind);
            }
        }
    }
}
