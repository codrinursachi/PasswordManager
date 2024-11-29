using PasswordManager.CustomControls;
using PasswordManager.Interfaces;
using PasswordManager.Utilities;
using System.Windows;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for PasswordCreationView.xaml
    /// </summary>
    public partial class PasswordCreationView : Window
    {
        public PasswordCreationView(
            IUserControlProviderService userControlProviderService)
        {
            InitializeComponent();
            MouseMove += AutoLocker.OnActivity;
            KeyDown += AutoLocker.OnActivity;
            var passwordModelEditor = userControlProviderService.ProvideUserControl<PasswordModelEditor>();
            pwdCreator.Content = passwordModelEditor;
        }
    }
}
