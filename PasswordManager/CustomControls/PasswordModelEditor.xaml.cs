using PasswordManager.DTO;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.ViewModels;
using PasswordManager.ViewModels.CustomControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManager.CustomControls
{
    /// <summary>
    /// Interaction logic for PasswordModelEditor.xaml
    /// </summary>
    public partial class PasswordModelEditor : UserControl
    {
        public static readonly DependencyProperty AddButtonVisibleProperty = DependencyProperty.Register("AddButtonVisible", typeof(bool), typeof(PasswordModelEditor));

        public PasswordModelEditor(IDataContextProviderService dataContextProviderService)
        {
            InitializeComponent();
            DataContext=dataContextProviderService.ProvideDataContext<PasswordModelEditorViewModel>();
        }

        public bool AddButtonVisible
        {
            get { return (bool)GetValue(AddButtonVisibleProperty); }
            set { SetValue(AddButtonVisibleProperty, value); }
        }

        private void datePickerGotMouseCapture(object sender, MouseEventArgs e)
        {
            datePicker.Foreground = Brushes.Black;
        }

        private void datePickerKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (string.IsNullOrEmpty(datePicker.Text))
                {
                    datePicker.SelectedDate = DateTime.Today;
                    datePicker.Foreground = Brushes.Transparent;
                }
            }
        }

        private void txtTagKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                txtTagLostFocus(sender, null);
            }
        }

        private void txtTagLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
            {
                return;
            }

            string tag = "#" + ((TextBox)sender).Text.Trim().Trim('#');
            ((PasswordModelEditorViewModel)DataContext).CompletedTags.Add(tag);
            ((TextBox)sender).Text = string.Empty;
        }

        private void TagsMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Tags.SelectedItem == null)
            {
                return;
            }
            ((PasswordModelEditorViewModel)DataContext).CompletedTags.Remove(Tags.SelectedItem.ToString());
        }
    }
}
