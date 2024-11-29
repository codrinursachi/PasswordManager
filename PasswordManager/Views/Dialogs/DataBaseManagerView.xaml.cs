using PasswordManager.Interfaces;
using PasswordManager.Utilities;
using PasswordManager.ViewModels.Dialogs;
using System.Windows;

namespace PasswordManager.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for DatabaseManagerView.xaml
    /// </summary>
    public partial class DatabaseManagerView : Window
    {
        public DatabaseManagerView(
            DatabaseManagerViewModel databaseManagerViewModel)
        {
            InitializeComponent();
            MouseMove += AutoLocker.OnActivity;
            KeyDown += AutoLocker.OnActivity;
            DataContext = databaseManagerViewModel;
        }
    }
}
