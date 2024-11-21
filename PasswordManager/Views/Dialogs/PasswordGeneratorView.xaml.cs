using PasswordManager.Interfaces;
using PasswordManager.Utilities;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
