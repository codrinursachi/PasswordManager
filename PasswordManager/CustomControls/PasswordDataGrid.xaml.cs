using PasswordManager.DTO;
using PasswordManager.DTO.Extensions;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Utilities;
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
using System.Windows.Threading;

namespace PasswordManager.CustomControls
{
    /// <summary>
    /// Interaction logic for PasswordListView.xaml
    /// </summary>
    public partial class PasswordDataGrid : UserControl
    {
        public static readonly DependencyProperty PasswordListProperty = DependencyProperty.Register("PasswordList", typeof(ObservableCollection<PasswordToShowDTO>), typeof(PasswordDataGrid));
        public static readonly DependencyProperty DBPassProperty = DependencyProperty.Register("DBPass", typeof(byte[]), typeof(PasswordDataGrid));

        private char[] storedPass;
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

        public byte[] DBPass
        {
            get { return (byte[])GetValue(DBPassProperty); }
            set { SetValue(DBPassProperty, value); }
        }

        private void SetupTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (Clipboard.GetText().ToCharArray() == storedPass)
            {
                Clipboard.Clear();
            }

            Array.Clear(storedPass);
            timer.Stop();
        }

        private void pwdList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (PasswordToShowDTO)pwdList.SelectedItem;
            if (selectedItem == null)
            {
                return;
            }
            ((IPasswordSettable)Resources["ViewModel"]).DBPass = DBPass;
            ((IDatabaseChangeable)Resources["ViewModel"]).Database = ((IDatabaseChangeable)DataContext).Database[..^".json".Length];
            ((PasswordDataGridViewModel)Resources["ViewModel"]).PasswordModel = selectedItem.ToPasswordModel();
        }

        private void MenuItemUsername_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetDataObject(((PasswordToShowDTO)pwdList.SelectedItem).Username);
        }

        private void MenuItemPassword_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            storedPass = GetPasswordClearText.GetPasswordClearTextById(((PasswordToShowDTO)pwdList.SelectedItem).Id, ((IDatabaseChangeable)DataContext).Database, DBPass);
            Clipboard.SetDataObject(new string(storedPass));
            SetupTimer();
        }

        private void MenuItemDelete_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DeletePassword.DeletePasswordById(((PasswordToShowDTO)pwdList.SelectedItem).Id, ((IDatabaseChangeable)DataContext).Database, DBPass);
            PasswordList.Remove((PasswordToShowDTO)pwdList.SelectedItem);
        }
    }
}
