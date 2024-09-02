using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace PasswordManager.CustomControls
{
    public partial class PanelControlBar : UserControl
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        public static readonly DependencyProperty MaximizeVisibleProperty = DependencyProperty.Register("MaximizeVisible", typeof(bool), typeof(PanelControlBar));
        public static readonly DependencyProperty IsMainWindowProperty = DependencyProperty.Register("IsMainWindow", typeof(bool), typeof(PanelControlBar));
        public static readonly DependencyProperty WindowTitleProperty = DependencyProperty.Register("WindowTitle", typeof(string), typeof(PanelControlBar));
        public PanelControlBar()
        {
            InitializeComponent();
        }

        public bool MaximizeVisible
        {
            get { return (bool)GetValue(MaximizeVisibleProperty); }
            set { SetValue(MaximizeVisibleProperty, value); }
        }

        public bool IsMainWindow
        {
            get { return (bool)GetValue(IsMainWindowProperty); }
            set { SetValue(IsMainWindowProperty, value); }
        }

        public string WindowTitle
        {
            get { return (string)GetValue(WindowTitleProperty); }
            set { SetValue(WindowTitleProperty, value); }
        }

        private void pnlControlBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(Window.GetWindow(this));
            SendMessage(helper.Handle, 161, 2, 0);
        }

        private void pnlControlBarMouseEnter(object sender, MouseEventArgs e)
        {
            Window.GetWindow(this).MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void btnCloseClick(object sender, RoutedEventArgs e)
        {
            if (IsMainWindow)
            {
                Application.Current.Shutdown();
            }
            Window.GetWindow(this).Close();
        }

        private void btnMaximizeClick(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this).WindowState == WindowState.Normal)
            {
                Window.GetWindow(this).WindowState = WindowState.Maximized;
            }
            else
            {
                Window.GetWindow(this).WindowState = WindowState.Normal;
            }
        }

        private void btnMinimizeClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }
    }
}
