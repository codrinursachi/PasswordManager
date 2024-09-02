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
            ((PasswordCreationViewModel)this.DataContext).CloseAction = Close;
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
            if (e.Key == Key.Space)
            {
                if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
                {
                    return;
                }
                string tag = ((TextBox)sender).Text.Trim();
                ((PasswordCreationViewModel)this.DataContext).CompletedTags.Add(tag);
                ((TextBox)sender).Text = "";
            }
        }

        private void txtTagLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
            {
                return;
            }

            string tag = ((TextBox)sender).Text.Trim();
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

        private void txtCat_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((ComboBox)sender).ItemsSource = null;
            ((ComboBox)sender).ItemsSource = ((PasswordCreationViewModel)this.DataContext).CategoryPaths;
            ((TextBox)((ComboBox)sender).Template.FindName("PART_EditableTextBox", (ComboBox)sender)).Focus();
        }

        private void ComboBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)((ComboBox)sender).Template.FindName("PART_EditableTextBox", (ComboBox)sender)).Focus();
        }
    }
}
