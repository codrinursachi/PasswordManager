using PasswordManager.CustomControls;
using PasswordManager.Interfaces;
using PasswordManager.Services;
using System.Windows;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for PasswordCreationView.xaml
    /// </summary>
    public partial class PasswordCreationView : Window
    {
        public PasswordCreationView(
            IUserControlProviderService userControlProviderService,
            IAutoLockerService autoLockerService)
        {
            InitializeComponent();
            MouseMove += autoLockerService.OnActivity;
            KeyDown += autoLockerService.OnActivity;
            var passwordModelEditor = userControlProviderService.ProvideUserControl<PasswordModelEditor>();
            pwdCreator.Content = passwordModelEditor;
        }
    }
}
