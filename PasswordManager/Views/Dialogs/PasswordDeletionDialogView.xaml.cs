using PasswordManager.ViewModels.Dialogs;
using System.Windows.Controls;

namespace PasswordManager.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for PasswordDeletionDialogView.xaml
    /// </summary>
    public partial class PasswordDeletionDialogView : UserControl
    {
        public PasswordDeletionDialogView(
            PasswordDeletionDialogViewModel passwordDeletionDialogViewModel)
        {
            InitializeComponent();
            DataContext = passwordDeletionDialogViewModel;
        }
    }
}
