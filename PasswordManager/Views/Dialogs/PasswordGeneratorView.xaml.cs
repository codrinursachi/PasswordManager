using PasswordManager.Interfaces;
using PasswordManager.Services;
using PasswordManager.ViewModels;
using System.Windows;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for PasswordGeneratorView.xaml
    /// </summary>
    public partial class PasswordGeneratorView : Window
    {
        public PasswordGeneratorView(
            PasswordGeneratorViewModel passwordGeneratorViewModel,
            IAutoLockerService autoLockerService)
        {
            InitializeComponent();
            MouseMove += autoLockerService.OnActivity;
            KeyDown += autoLockerService.OnActivity;
            DataContext = passwordGeneratorViewModel;
        }
    }
}
