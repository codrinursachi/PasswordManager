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
    public class PasswordCreationViewModel : ViewModelBase, IDatabaseChangeable, IPasswordSettable
    {
        List<string> categoryPaths;
        string tags;
        string database;
        private bool overlayVisibility;
        private PasswordRepository passwordRepository;
        private string username;
        private string password;
        private string url;
        private DateTime expirationDate;
        private string categoryPath;
        private bool favorite;
        private string notes;
        private bool addButtonVisible;
        private bool editButtonVisible;

        public PasswordCreationViewModel()
        {
            CategoryPaths = new();
            ShowPasswordGeneratorViewCommand = new ViewModelCommand(ExecuteShowPasswordGeneratorCommand);
            DatabaseItems = new List<string>();
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases");
            foreach (var db in Directory.GetFiles(path))
            {
                DatabaseItems.Add(db[(path + "\\").Length..^".json".Length]);
            }

            AddPasswordCommand = new ViewModelCommand(ExecuteAddPasswordCommand);
            AddButtonVisible = true;
        }
        public ICommand AddPasswordCommand { get; }
        public ICommand ShowPasswordGeneratorViewCommand { get; }
        public bool AddButtonVisible
        {
            get => addButtonVisible; 
            set
            {
                addButtonVisible = value;
                editButtonVisible=!value;
                OnPropertyChanged(nameof(AddButtonVisible));
            }
        }

        public bool EditButtonVisible
        {
            get => editButtonVisible;
            set
            {
                editButtonVisible = value;
                OnPropertyChanged(nameof(EditButtonVisible));
            }
        }

        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
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
                OnPropertyChanged(nameof(Url));
            }
        }
        public DateTime ExpirationDate
        {
            get => expirationDate; 
            set
            {
                expirationDate = value;
                OnPropertyChanged(nameof(ExpirationDate));
            }
        }
        public string CategoryPath
        {
            get => categoryPath; 
            set
            {
                categoryPath = value;
                OnPropertyChanged(nameof(CategoryPath));
            }
        }

        public List<string> CategoryPaths
        {
            get => categoryPaths;
            set
            {
                categoryPaths = value;
                OnPropertyChanged(nameof(CategoryPaths));
            }
        }
        public ObservableCollection<string> CompletedTags { get; set; } = [];
        public string Tags
        {
            get => tags;
            set
            {
                tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }
        public bool Favorite
        {
            get => favorite; set
            {
                favorite = value;
                OnPropertyChanged(nameof(Favorite));
            }
        }
        public string Database
        {
            get => database;
            set
            {
                database = value;
                OnPropertyChanged(nameof(Database));
                CategoryPaths.Clear();
                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", value + ".json")))
                {
                    passwordRepository = new(Database + ".json", DBPass);
                    CategoryPaths.AddRange(passwordRepository.GetAllPasswords().Select(p => p.CategoryPath).Distinct().Where(p => p != null).ToList());
                }
            }
        }
        public string Notes
        {
            get => notes; 
            set
            {
                notes = value;
                OnPropertyChanged(nameof(Notes));
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

        public byte[] DBPass { get; set; }

        public void ExecuteAddPasswordCommand(object obj)
        {
            string tags = string.Join(" ", CompletedTags);
            PasswordModel newPassword = new() { Username = Username, Password = Password.ToCharArray(), Url = Url, ExpirationDate = ExpirationDate, CategoryPath = CategoryPath, Tags = tags, Favorite = Favorite, Notes = Notes };
            PasswordRepository passwordRepository = new(Database + ".json", DBPass);
            passwordRepository.Add(newPassword);
            CloseAction?.Invoke();
        }

        private void ExecuteShowPasswordGeneratorCommand(object obj)
        {
            var PasswordGen = new PasswordGeneratorView();
            OverlayVisibility = true;
            PasswordGen.ShowDialog();
            OverlayVisibility = false;
        }
    }
}
