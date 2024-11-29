using PasswordManager.Interfaces;
using PasswordManager.ViewModels.Dialogs;
using System.Windows;

namespace PasswordManager.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for PasswordDeletionDialogView.xaml
    /// </summary>
    public partial class PasswordDeletionDialogView : Window
    {
        public PasswordDeletionDialogView(
            PasswordDeletionDialogViewModel passwordDeletionDialogViewModel,
            IAutoLockerService autoLockerService)
        {
            InitializeComponent();
            MouseMove += autoLockerService.OnActivity;
            KeyDown += autoLockerService.OnActivity;
            DataContext = passwordDeletionDialogViewModel;
        }
    }
}
