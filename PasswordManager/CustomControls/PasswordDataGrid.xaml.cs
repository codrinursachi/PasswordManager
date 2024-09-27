using PasswordManager.DTO;
using PasswordManager.DTO.Extensions;
using PasswordManager.Interfaces;
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
        public static readonly DependencyProperty DatabaseProperty = DependencyProperty.Register("Database", typeof(string), typeof(PasswordDataGrid));
        public static readonly DependencyProperty DBPassProperty = DependencyProperty.Register("DBPass", typeof(byte[]), typeof(PasswordDataGrid));

        private string storedPass;
        private DispatcherTimer timer;

        public PasswordDataGrid()
        {
            InitializeComponent();
        }

        public ObservableCollection<PasswordToShowDTO> PasswordList
        {
            get { return (ObservableCollection<PasswordToShowDTO>)GetValue(PasswordListProperty); }
            set { SetValue(PasswordListProperty, value); }
        }

        public string Database
        {
            get { return (string)GetValue(DatabaseProperty); }
            set { SetValue(DatabaseProperty, value); }
        }

        public byte[] DBPass
        {
            get { return (byte[])GetValue(DBPassProperty); }
            set { SetValue(DBPassProperty, value); }
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


        private void cpyClipboardMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            storedPass = GetPasswordClearText.GetPasswordClearTextById(((PasswordToShowDTO)pwdList.SelectedItem).Id, Database, ((IPasswordSettable)(Window.GetWindow(this)).DataContext).DBPass);
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
            pass.Password = GetPasswordClearText.GetPasswordClearTextById((pass).Id, Database, ((IPasswordSettable)(Window.GetWindow(this)).DataContext).DBPass);
            pwdList.Items.Refresh();
            ClearPass(pass);
        }

        private async void ClearPass(PasswordToShowDTO pass)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            pass.Password = "********";
            pwdList.CancelEdit();
            pwdList.CancelEdit();
            pwdList.Items.Refresh();
        }

        private void delPwd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DeletePassword.DeletePasswordById(((PasswordToShowDTO)pwdList.SelectedItem).Id, Database, DBPass);
            PasswordList.Remove((PasswordToShowDTO)pwdList.SelectedItem);
        }

        private void pwdList_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (this.pwdList.SelectedItem != null)
            {
                ((DataGrid)sender).RowEditEnding -= pwdList_RowEditEnding;
                ((DataGrid)sender).CommitEdit();
                ((DataGrid)sender).Items.Refresh();
                ((DataGrid)sender).RowEditEnding += pwdList_RowEditEnding;
                var pass = (PasswordToShowDTO)pwdList.SelectedItem;
                if (pass.Password == "********")
                {
                    pass.Password = GetPasswordClearText.GetPasswordClearTextById(pass.Id, Database, DBPass);
                }

                EditPassword.EditPasswordById(pass.ToPasswordModel(), Database, DBPass);
                pass.Password = "********";
                ((DataGrid)sender).Items.Refresh();
            }
        }
    }
}
