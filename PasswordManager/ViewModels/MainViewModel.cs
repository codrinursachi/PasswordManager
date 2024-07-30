using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PasswordManager.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        private string _caption;
        public MainViewModel()
        {
            ShowAllPasswordsView = new ViewModelCommand(ExecuteShowAllPasswordsView);
            ShowFavoritesView = new ViewModelCommand(ExecuteShowFavoritesView);
            ShowLabelsView = new ViewModelCommand(ExecuteShowLabelsView);
            ShowDirectoryView = new ViewModelCommand(ExecuteShowDirectoryView);

            ExecuteShowAllPasswordsView(null);
        }

        public ICommand ShowAllPasswordsView { get; }
        public ICommand ShowFavoritesView { get; }
        public ICommand ShowLabelsView { get; }
        public ICommand ShowDirectoryView { get; }

        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        internal ViewModelBase CurrentChildView
        {
            get => _currentChildView;
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        private void ExecuteShowDirectoryView(object obj)
        {
            CurrentChildView = new DirectoryViewModel();
            Caption = "Directories";
        }

        private void ExecuteShowLabelsView(object obj)
        {
            CurrentChildView = new LabelsViewModel();
            Caption = "Labels";
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
