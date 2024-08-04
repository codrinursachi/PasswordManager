using PasswordManager.DTO;
using PasswordManager.DTO.Extensions;
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
    class FavoritesViewModel:ViewModelBase
    {
        public ICommand RefreshCommand { get; }
        public ObservableCollection<PasswordToShowDTO> Passwords { get; }
        IPasswordRepository passwordRepository;
        public FavoritesViewModel()
        {
            RefreshCommand = new ViewModelCommand(ExecuteRefreshCommand);
            passwordRepository = new PasswordRepository();
            Passwords = new(passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).Where(p => p.Favorite).Select(p=>p.ToPasswordToShow()));
        }

        private void ExecuteRefreshCommand(object obj)
        {
            Passwords.Clear();
            foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).Where(p=>p.Favorite).Select(p => p.ToPasswordToShow()))
            {
                Passwords.Add(password);
            }
        }
    }
}
