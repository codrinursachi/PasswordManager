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
    public partial class PasswordListView : UserControl
    {
        public static readonly DependencyProperty PasswordListProperty = DependencyProperty.Register("PasswordList", typeof(ObservableCollection<PasswordToShowDTO>), typeof(PasswordListView));
        private string _storedPass;
        private DispatcherTimer _timer;

        public PasswordListView()
        {
            InitializeComponent();            
        }

        private void SetupTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Clipboard.GetText()==_storedPass)
            {
                Clipboard.Clear();
            }

            _timer.Stop();
        }

        public ObservableCollection<PasswordToShowDTO> PasswordList
        {
            get { return (ObservableCollection<PasswordToShowDTO>)GetValue(PasswordListProperty); }
            set { SetValue(PasswordListProperty, value); }
        }

        private void cpyClipboard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _storedPass= GetPasswordClearText.GetPasswordClearTextById(((PasswordToShowDTO)pwdList.SelectedItem).Id);
            Clipboard.SetDataObject(_storedPass);
            SetupTimer();
        }

        private void showPwd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((PasswordToShowDTO)pwdList.SelectedItem).Password = GetPasswordClearText.GetPasswordClearTextById(((PasswordToShowDTO)pwdList.SelectedItem).Id);
            pwdList.Items.Refresh();
            ClearPass();            
        }

        private async Task ClearPass()
        {
            var passToClear = (PasswordToShowDTO)pwdList.SelectedItem;
            await Task.Delay(TimeSpan.FromSeconds(5));
            passToClear.Password= "********";
            pwdList.Items.Refresh();
        }
    }
}
