using PasswordManager.CustomControls;
using PasswordManager.Interfaces;
using PasswordManager.Utilities;
using PasswordManager.ViewModels;
using PasswordManager.ViewModels.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for PasswordCreationView.xaml
    /// </summary>
    public partial class PasswordCreationView : Window
    {
        public PasswordCreationView(IUserControlProviderService userControlProviderService)
        {
            InitializeComponent();
            MouseMove += AutoLocker.OnActivity;
            KeyDown += AutoLocker.OnActivity;
            var passwordModelEditor = userControlProviderService.ProvideUserControl<PasswordModelEditor>();
            pwdCreator.Content=passwordModelEditor;
        }
    }
}
