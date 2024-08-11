using PasswordManager.ViewModels;
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
        private static bool isLoaded = false;
        public PasswordCreationView()
        {
            InitializeComponent();
            ((PasswordCreationViewModel)this.DataContext).CloseAction = UnLoad;
        }

        public void Load()
        {
            if (!isLoaded)
            {
                Show();
                isLoaded = true;
            }
        }

        public void UnLoad()
        {
            isLoaded = false;
            Close();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            isLoaded = false;
            Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void datePicker_GotMouseCapture(object sender, MouseEventArgs e)
        {
            datePicker.Foreground = Brushes.DarkGray;
            datePicker.SelectedDate = DateTime.Today;
        }

        private void datePicker_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (string.IsNullOrEmpty(datePicker.Text))
                {
                    datePicker.SelectedDate = default(DateTime);
                    datePicker.Foreground = Brushes.Transparent;
                }
            }
        }

        private void txtTag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                txtTag.Text = txtTag.Text[^1] == ' ' ? txtTag.Text : txtTag.Text + " ";
                txtTag.CaretIndex = txtTag.Text.Length;
            }
        }
    }
}
