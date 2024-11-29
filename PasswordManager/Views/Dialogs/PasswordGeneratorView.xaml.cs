using PasswordManager.Interfaces;
using PasswordManager.Utilities;
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
            PasswordGeneratorViewModel passwordGeneratorViewModel)
        {
            InitializeComponent();
            MouseMove += AutoLocker.OnActivity;
            KeyDown += AutoLocker.OnActivity;
            DataContext = passwordGeneratorViewModel;
        }
    }
}
