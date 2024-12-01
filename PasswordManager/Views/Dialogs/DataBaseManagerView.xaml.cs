using PasswordManager.ViewModels.Dialogs;
using System.Windows.Controls;

namespace PasswordManager.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for DatabaseManagerView.xaml
    /// </summary>
    public partial class DatabaseManagerView : UserControl
    {
        public DatabaseManagerView(
            DatabaseManagerViewModel databaseManagerViewModel)
        {
            InitializeComponent();
            DataContext = databaseManagerViewModel;
        }
    }
}
