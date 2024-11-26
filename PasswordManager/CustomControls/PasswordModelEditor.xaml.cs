using PasswordManager.Interfaces;
using PasswordManager.ViewModels.CustomControls;
using System.Windows.Controls;

namespace PasswordManager.CustomControls
{
    /// <summary>
    /// Interaction logic for PasswordModelEditor.xaml
    /// </summary>
    public partial class PasswordModelEditor : UserControl
    {
        public PasswordModelEditor(
            IDataContextProviderService dataContextProviderService)
        {
            InitializeComponent();
            DataContext = dataContextProviderService.ProvideDataContext<PasswordModelEditorViewModel>();
        }
    }
}
