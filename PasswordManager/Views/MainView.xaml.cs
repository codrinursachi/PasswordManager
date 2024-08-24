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

        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }

        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
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

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnAddPass_Click(object sender, RoutedEventArgs e)
        {
            PasswordCreationView passwordCreationView = new();
            Overlay.Visibility = Visibility.Visible;
            passwordCreationView.ShowDialog();
            Overlay.Visibility=Visibility.Hidden;
        }

        private void dtbSelector_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((MainViewModel)this.DataContext).GetDatabases();
            CollectionViewSource.GetDefaultView(dtbSelector)?.Refresh();
            App.Current.Properties["ShouldRefresh"] = true;
        }
    }
}