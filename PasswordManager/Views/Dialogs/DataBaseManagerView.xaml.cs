using PasswordManager.Interfaces;
using PasswordManager.Services;
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
            DatabaseManagerViewModel databaseManagerViewModel, 
            IAutoLockerService autoLockerService)
        {
            InitializeComponent();
            MouseMove += autoLockerService.OnActivity;
            KeyDown += autoLockerService.OnActivity;
            DataContext = databaseManagerViewModel;
        }
    }
}
