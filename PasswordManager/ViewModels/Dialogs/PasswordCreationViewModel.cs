using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.Utilities;
using PasswordManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace PasswordManager.ViewModels
{
    public class PasswordCreationViewModel : ViewModelBase, IDatabaseChangeable, IPasswordSettable, IPasswordPair
    {
        List<string> categoryPaths;
        string tags;
        string database;
        private bool overlayVisibility;
        private PasswordRepository passwordRepository;
        private string username;
        private string password;
        private string url;
        private DateTime expirationDate = DateTime.Today;
        private string categoryPath;
        private bool favorite;
        private string notes;
        private bool addButtonVisible;
        private bool editButtonVisible;
        private string urlErrorMessage;
        private string usernameErrorMessage;
        private string passwordErrorMessage;

        public PasswordCreationViewModel()
        {
            CategoryPaths = [];
            ShowPasswordGeneratorViewCommand = new ViewModelCommand(ExecuteShowPasswordGeneratorCommand);
            DatabaseItems = [];
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases");
            foreach (var db in Directory.GetFiles(path))
            {
                DatabaseItems.Add(db[(path + "\\").Length..^".json".Length]);
            }

            AddPasswordCommand = new ViewModelCommand(ExecuteAddPasswordCommand);
            EditPasswordCommand = new ViewModelCommand(ExecuteEditPasswordCommand);
            AddButtonVisible = true;
        }
        public ICommand AddPasswordCommand { get; }
        public ICommand EditPasswordCommand { get; }
        public ICommand ShowPasswordGeneratorViewCommand { get; }
        public bool AddButtonVisible
        {
            get => addButtonVisible;
            set
            {
                addButtonVisible = value;
                editButtonVisible = !value;
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

        public int Id { get; set; }

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

        public char[] PasswordAsCharArray { get; set; } = [];
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
        public string InitialDatabase { get; set; }
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
        public string UrlErrorMessage
        {
            get => urlErrorMessage;
            set
            {
                urlErrorMessage = value;
                OnPropertyChanged(nameof(UrlErrorMessage));
            }
        }
        public string UsernameErrorMessage
        {
            get => usernameErrorMessage;
            set
            {
                usernameErrorMessage = value;
                OnPropertyChanged(nameof(UsernameErrorMessage));
            }
        }
        public string PasswordErrorMessage
        {
            get => passwordErrorMessage;
            set
            {
                passwordErrorMessage = value;
                OnPropertyChanged(nameof(PasswordErrorMessage));
            }
        }

        public void ExecuteAddPasswordCommand(object obj)
        {
            if (!Validate())
            {
                return;
            }

            string tags = string.Join(" ", CompletedTags);
            PasswordModel newPassword = new() { Username = Username, Password = PasswordAsCharArray, Url = Url, ExpirationDate = ExpirationDate == DateTime.Today ? default : ExpirationDate, CategoryPath = CategoryPath, Tags = tags, Favorite = Favorite, Notes = Notes };
            PasswordRepository passwordRepository = new(Database + ".json", DBPass);
            passwordRepository.Add(newPassword);
            Array.Fill(PasswordAsCharArray, '0');
            CloseAction?.Invoke();
        }

        public void ExecuteEditPasswordCommand(object obj)
        {
            if (!Validate())
            {
                return;
            }

            string tags = string.Join(" ", CompletedTags);
            PasswordModel newPassword = new() { Username = Username, Password = PasswordAsCharArray, Url = Url, ExpirationDate = ExpirationDate == DateTime.Today ? default : ExpirationDate, CategoryPath = CategoryPath, Tags = tags, Favorite = Favorite, Notes = Notes };
            PasswordRepository passwordRepository = new(Database + ".json", DBPass);
            if (InitialDatabase != Database)
            {
                DeletePassword.DeletePasswordById(Id, InitialDatabase + ".json", DBPass);
                passwordRepository.Add(newPassword);
            }
            else
            {
                passwordRepository.Edit(Id, newPassword);
            }

            Password = string.Empty;
            Array.Fill(PasswordAsCharArray, '0');
            ((IRefreshable)obj).Refresh();
        }

        private bool Validate()
        {
            UrlErrorMessage = !string.IsNullOrEmpty(Url) && Uri.IsWellFormedUriString(Url, UriKind.RelativeOrAbsolute) ? string.Empty : "URL must be valid";
            UsernameErrorMessage = !string.IsNullOrEmpty(Username) && Username.Length >= 5 ? string.Empty : "Username must be longer than 4 characters";
            PasswordErrorMessage = PasswordAsCharArray.Length >= 5 ? string.Empty : "Password must be longer than 4 characters";
            return (UrlErrorMessage + UsernameErrorMessage + PasswordErrorMessage).Length == 0;
        }

        private void ExecuteShowPasswordGeneratorCommand(object obj)
        {
            var PasswordGen = new PasswordGeneratorView();
            OverlayVisibility = true;
            if (PasswordGen.ShowDialog() == true)
            {
                var passGenDataContext = (PasswordGeneratorViewModel)PasswordGen.DataContext;
                Password = string.Concat(Enumerable.Repeat('*', passGenDataContext.GeneratedPassword.Length));
                PasswordAsCharArray = passGenDataContext.GeneratedPassword;
            }
            OverlayVisibility = false;
        }
    }
}
