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
        public PasswordGeneratorView(IDataContextProviderService dataContextProviderService)
        {
            InitializeComponent();
            MouseMove += AutoLocker.OnActivity;
            KeyDown += AutoLocker.OnActivity;
            DataContext = dataContextProviderService.ProvideDataContext<PasswordGeneratorViewModel>();
        }
    }
}
