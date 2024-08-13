using PasswordManager.Models;
using PasswordManager.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        List<string> _categoryPaths;
        string _tags;
        bool _favorite;
        string _database;
        string _notes;

        public PasswordCreationViewModel()
        {
            var passwordRepository = new PasswordRepository();
            DatabaseItems = new List<string>();
            foreach (var db in (Directory.GetFiles(Thread.CurrentPrincipal.Identity.Name)))
            {
                DatabaseItems.Add(db[(Thread.CurrentPrincipal.Identity.Name + "\\").Length..]);
            }
            AddPasswordCommand = new ViewModelCommand(ExecuteAddPasswordCommand);
            CategoryPaths = passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).Select(p => p.CategoryPath).Distinct().Where(p => p != null).ToList();
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
            }
        }
        public string Url
        {
            get => _url;
            set
            {
                _url = value;
            }
        }
        public DateTime ExpirationDate
        {
            get => _expirationDate;
            set
            {
                _expirationDate = value;
            }
        }
        public string CategoryPath { get; set; }

        public List<string> CategoryPaths
        {
            get => _categoryPaths;
            set
            {
                _categoryPaths = value;
                OnPropertyChanged(nameof(CategoryPaths));
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
        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
            }
        }
        public List<string> DatabaseItems { get; }
        public Action CloseAction { get; set; }
        private void ExecuteAddPasswordCommand(object obj)
        {
            App.Current.Properties["SelectedDb"] = Database;
            PasswordModel newPassword = new() { Username = Username, Password = Password, Url = Url, ExpirationDate = ExpirationDate, CategoryPath = CategoryPath, Tags = Tags, Favorite = Favorite, Notes = Notes };
            var repository = new PasswordRepository();
            repository.Add(newPassword, App.Current.Properties["pass"].ToString());
            App.Current.Properties["ShouldRefresh"] = true;
            CloseAction.Invoke();
        }
    }
}
