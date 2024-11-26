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
        public PasswordDeletionDialogView(IDataContextProviderService dataContextProviderService)
        {
            InitializeComponent();
            DataContext = dataContextProviderService.ProvideDataContext<PasswordDeletionDialogViewModel>();
        }
    }
}
