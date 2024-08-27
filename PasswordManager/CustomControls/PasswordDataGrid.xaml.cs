using PasswordManager.DTO;
using PasswordManager.Models;
using PasswordManager.Utilities;
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
using System.Windows.Threading;

namespace PasswordManager.CustomControls
{
    /// <summary>
    /// Interaction logic for PasswordListView.xaml
    /// </summary>
    public partial class PasswordDataGrid : UserControl
    {
        public static readonly DependencyProperty PasswordListProperty = DependencyProperty.Register("PasswordList", typeof(ObservableCollection<PasswordToShowDTO>), typeof(PasswordDataGrid));
        private string storedPass;
        private DispatcherTimer timer;

        public PasswordDataGrid()
        {
            InitializeComponent();
        }

        private void SetupTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (Clipboard.GetText() == storedPass)
            {
                Clipboard.Clear();
            }

            timer.Stop();
        }

        public ObservableCollection<PasswordToShowDTO> PasswordList
        {
            get { return (ObservableCollection<PasswordToShowDTO>)GetValue(PasswordListProperty); }
            set { SetValue(PasswordListProperty, value); }
        }

        private void cpyClipboardMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            storedPass = GetPasswordClearText.GetPasswordClearTextById(((PasswordToShowDTO)pwdList.SelectedItem).Id);
            if (storedPass != null)
            {
                Clipboard.SetDataObject(storedPass);
                SetupTimer();
            }
        }

        private void showPwdMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var pass = (PasswordToShowDTO)pwdList.SelectedItem;
            pwdList.SelectedItem = null;
            pass.Password = GetPasswordClearText.GetPasswordClearTextById((pass).Id);
            pwdList.Items.Refresh();
            ClearPass(pass);
        }

        private async void ClearPass(PasswordToShowDTO pass)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            pass.Password = "********";
            pwdList.Items.Refresh();
        }
    }
}
