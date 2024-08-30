using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.Views;
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
    public class PasswordCreationViewModel : ViewModelBase,IDatabaseChangeable
    {
        string username;
        string password;
        string url;
        DateTime expirationDate;
        List<string> categoryPaths;
        string tags;
        bool favorite;
        string database;
        string notes;
        private bool overlayVisibility;

        public PasswordCreationViewModel()
        {
            OverlayVisibility = false;
            ShowPasswordGeneratorViewCommand = new ViewModelCommand(ExecuteShowPasswordGeneratorCommand);
            PasswordRepository passwordRepository = new(Database, App.Current.Properties["pass"].ToString());
            DatabaseItems = new List<string>();
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases");
            foreach (var db in Directory.GetFiles(path))
            {
                DatabaseItems.Add(db[(path + "\\").Length..^".json".Length]);
            }
            AddPasswordCommand = new ViewModelCommand(ExecuteAddPasswordCommand);
            CategoryPaths = passwordRepository.GetAllPasswords().Select(p => p.CategoryPath).Distinct().Where(p => p != null).ToList();
        }
        public ICommand AddPasswordCommand { get; }
        public ICommand ShowPasswordGeneratorViewCommand { get; }
        public string Username
        {
            get => username;
            set
            {
                username = value;
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string Url
        {
            get => url;
            set
            {
                url = value;
            }
        }
        public DateTime ExpirationDate
        {
            get => expirationDate;
            set
            {
                expirationDate = value;
            }
        }
        public string CategoryPath { get; set; }

        public List<string> CategoryPaths
        {
            get => categoryPaths;
            set
            {
                categoryPaths = value;
                OnPropertyChanged(nameof(CategoryPaths));
            }
        }
        public ObservableCollection<string> CompletedTags { get; set; } = new();
        public string Tags
        {
            get => tags;
            set
            {
                tags = value;
                OnPropertyChanged(nameof(tags));
            }
        }
        public bool Favorite
        {
            get => favorite;
            set
            {
                favorite = value;
            }
        }
        public string Database
        {
            get => database;
            set
            {
                database = value;
                OnPropertyChanged(nameof(Database));
            }
        }
        public string Notes
        {
            get => notes;
            set
            {
                notes = value;
            }
        }
        public List<string> DatabaseItems { get; }
        public Action CloseAction { get; set; }
        public bool OverlayVisibility
        {
            get => overlayVisibility; 
            set
            {
                overlayVisibility = value;
                OnPropertyChanged(nameof(OverlayVisibility));
            }
        }

        private void ExecuteAddPasswordCommand(object obj)
        {
            string tags = string.Join(" ", CompletedTags);
            PasswordModel newPassword = new() { Username = Username, Password = Password, Url = Url, ExpirationDate = ExpirationDate, CategoryPath = CategoryPath, Tags = tags, Favorite = Favorite, Notes = Notes };
            PasswordRepository passwordRepository = new(Database, App.Current.Properties["pass"].ToString());
            passwordRepository.Add(newPassword);
            CloseAction.Invoke();
        }

        private void ExecuteShowPasswordGeneratorCommand(object obj)
        {
            var PasswordGen = new PasswordGeneratorView(this);
            OverlayVisibility = true;
            PasswordGen.ShowDialog();
            OverlayVisibility = false;
        }
    }
}
