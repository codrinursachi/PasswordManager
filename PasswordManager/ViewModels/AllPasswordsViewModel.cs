using PasswordManager.Models;
using PasswordManager.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PasswordManager.ViewModels
{
    class AllPasswordsViewModel:ViewModelBase
    {
        public ICommand RefreshCommand { get; }
        public ObservableCollection<PasswordModel> Passwords { get; private set; }
        IPasswordRepository passwordRepository;
        public AllPasswordsViewModel()
        {
            RefreshCommand = new ViewModelCommand(ExecuteRefreshCommand);
            passwordRepository = new PasswordRepository();
            Passwords = new(passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).OrderBy(p => p.Url));
        }

        private void ExecuteRefreshCommand(object obj)
        {
            Passwords.Clear();
            foreach(var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).OrderBy(p=>p.Url))
            {
                Passwords.Add(password);
            }
        }
    }
}
