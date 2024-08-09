using PasswordManager.Models;
using PasswordManager.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PasswordManager.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        private string _caption;
        private int _selectedDb;
        private DispatcherTimer _timer;
        public MainViewModel()
        {
            ShowAllPasswordsView = new ViewModelCommand(ExecuteShowAllPasswordsView);
            ShowFavoritesView = new ViewModelCommand(ExecuteShowFavoritesView);
            ShowLabelsView = new ViewModelCommand(ExecuteShowLabelsView);
            ShowCategoryView = new ViewModelCommand(ExecuteShowCategoryView);
            Databases = new();
            GetDatabases();
            SelectedDb = 0;
            ExecuteShowAllPasswordsView(null);
            SetupTimer();
            InputManager.Current.PreProcessInput += OnActivity;
        }

        private void OnActivity(object sender, PreProcessInputEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        public ICommand ShowAllPasswordsView { get; }
        public ICommand ShowFavoritesView { get; }
        public ICommand ShowLabelsView { get; }
        public ICommand ShowCategoryView { get; }

        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public ViewModelBase CurrentChildView
        {
            get => _currentChildView;
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        public ObservableCollection<string> Databases { get; set; }

        public int SelectedDb
        {
            get => _selectedDb;
            set
            {
                _selectedDb = value;
                App.Current.Properties["SelectedDb"] = Databases[_selectedDb];
                App.Current.Properties["ShouldRefresh"] = true;
            }
        }

        public void GetDatabases()
        {
            Databases.Clear();
            foreach (var db in (Directory.GetFiles(".").Where(file => Path.GetFileName(file).StartsWith(Thread.CurrentPrincipal?.Identity?.Name)).Select(p => p = p.Remove(0, (@".\" + Thread.CurrentPrincipal?.Identity?.Name).Length))))
            {
                Databases.Add(db);
            }
            App.Current.Properties["Databases"] = Databases;
        }

        private void SetupTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(60);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            App.Current.Properties["timeout"] = true;
            _timer.Stop();
            Application.Current.Shutdown();
        }
        private void ExecuteShowCategoryView(object obj)
        {
            CurrentChildView = new CategoryViewModel();
            Caption = "Categories";
        }

        private void ExecuteShowLabelsView(object obj)
        {
            CurrentChildView = new TagsViewModel();
            Caption = "Tags";
        }

        private void ExecuteShowFavoritesView(object obj)
        {
            CurrentChildView = new FavoritesViewModel();
            Caption = "Favorites";
        }

        private void ExecuteShowAllPasswordsView(object obj)
        {
            CurrentChildView = new AllPasswordsViewModel();
            Caption = "All Passwords";
        }
    }
}
