using PasswordManager.Models;
using PasswordManager.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PasswordManager.ViewModels
{
    internal class PasswordCreationViewModel : ViewModelBase
    {
        public ICommand AddPasswordCommand { get; }
        string _username;
        string _password;
        string _url;
        DateTime _expirationDate;
        string _categoryPath;
        string _tags;
        bool _favorite;
        string _database;
        string _notes;

        public PasswordCreationViewModel()
        {
            var passwordRepository = new PasswordRepository();
            DatabaseItems = new ObservableCollection<string>(passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).Select(p => p.Database).ToHashSet());
            AddPasswordCommand = new ViewModelCommand(ExecuteAddPasswordCommand);
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                OnPropertyChanged(nameof(Url));
            }
        }
        public DateTime ExpirationDate
        {
            get => _expirationDate;
            set
            {
                _expirationDate = value;
                OnPropertyChanged(nameof(ExpirationDate));
            }
        }
        public string CategoryPath
        {
            get => _categoryPath;
            set
            {
                _categoryPath = value;
                OnPropertyChanged(nameof(CategoryPath));
            }
        }
        public string Tags
        {
            get => _tags;
            set
            {
                _tags = value;
                OnPropertyChanged(nameof(_tags));
            }
        }
        public bool Favorite
        {
            get => _favorite;
            set
            {
                _favorite = value;
                OnPropertyChanged(nameof(Favorite));
            }
        }
        public string Database
        {
            get => _database;
            set
            {
                _database = value;
                OnPropertyChanged(nameof(Database));
            }
        }
        public string Notes { 
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }
        public ObservableCollection<string> DatabaseItems { get; }
        public Action CloseAction { get; set; }
        private void ExecuteAddPasswordCommand(object obj)
        {
            PasswordModel newPassword = new() { Username = Username, Password = Password, Url = Url, ExpirationDate = ExpirationDate, CategoryPath = CategoryPath, Tags = Tags, Favorite = Favorite, Database = Database, Notes = Notes };
            var repository = new PasswordRepository();
            repository.Add(newPassword, App.Current.Properties["pass"].ToString());
            CloseAction.Invoke();
        }
    }
}
