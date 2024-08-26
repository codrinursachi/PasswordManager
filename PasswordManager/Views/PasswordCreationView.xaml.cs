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
        public PasswordCreationView()
        {
            InitializeComponent();
        }

        private void btnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void pnlControlBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void datePickerGotMouseCapture(object sender, MouseEventArgs e)
        {
            datePicker.Foreground = Brushes.Black;
            datePicker.SelectedDate = DateTime.Today;
        }

        private void datePickerKeyUp(object sender, KeyEventArgs e)
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

        private void txtTagKeyUp(object sender, KeyEventArgs e)
        {
            string tag;
            if (e.Key == Key.Space)
            {
                tag = ((TextBox)sender).Text.Trim();
                ((PasswordCreationViewModel)this.DataContext).CompletedTags.Add(tag);
                ((TextBox)sender).Text = "";
            }
        }

        private void txtTagLostFocus(object sender, RoutedEventArgs e)
        {
            string tag;
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                return;
            }

            tag = ((TextBox)sender).Text.Trim();
            ((PasswordCreationViewModel)this.DataContext).CompletedTags.Add(tag);
            ((TextBox)sender).Text = "";
        }

        private void TagsMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Tags.SelectedItem == null)
            {
                return;
            }
            ((PasswordCreationViewModel)this.DataContext).CompletedTags.Remove(Tags.SelectedItem.ToString());
        }
    }
}
