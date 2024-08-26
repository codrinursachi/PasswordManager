using PasswordManager.ViewModels;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        public MainView()
        {
            InitializeComponent();
            this.MouseMove += ((MainViewModel)this.DataContext).OnActivity;
            this.KeyDown += ((MainViewModel)this.DataContext).OnActivity;
        }

        private void pnlControlBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }

        private void pnlControlBarMouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void btnCloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMaximizeClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void btnMinimizeClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnAddPassClick(object sender, RoutedEventArgs e)
        {
            PasswordCreationView passwordCreationView = new();
            Overlay.Visibility = Visibility.Visible;
            passwordCreationView.ShowDialog();
            Overlay.Visibility=Visibility.Hidden;
        }

        private void dtbSelectorMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((MainViewModel)this.DataContext).GetDatabases();
            CollectionViewSource.GetDefaultView(dtbSelector)?.Refresh();
            App.Current.Properties["ShouldRefresh"] = true;
        }
    }
}