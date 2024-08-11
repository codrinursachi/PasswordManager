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

namespace PasswordManager.ViewModels
{
    class TagsViewModel : ViewModelBase
    {
        private List<string> _filter = new();

        public TagsViewModel()
        {
            IPasswordRepository passwordRepository = new PasswordRepository();
            var tags = passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).Where(p => p.Tags != null).Select(p => p.Tags.Split());
            foreach (var itemTags in tags)
            {
                foreach (var tag in itemTags)
                {
                    Tags.Add(tag);
                }
            }

            Passwords = new(passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).Select(p => p.ToPasswordToShow()));
        }

        public ObservableCollection<PasswordToShowDTO> Passwords { get; }
        public HashSet<string> Tags { get; } = new();
        public List<string> Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                FilterPwd();
            }
        }

        private void FilterPwd()
        {
            IPasswordRepository passwordRepository = new PasswordRepository();
            Passwords.Clear();
            if (Filter.Count==0)
            {
                foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).Select(p => p.ToPasswordToShow()))
                {
                    Passwords.Add(password);
                }
            }
            else
            {
                foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString()).Select(p => p.ToPasswordToShow()).Where(p=>p.Tags!=null&&p.Tags.Split().ToHashSet().IsSupersetOf(Filter)))
                {
                    Passwords.Add(password);
                }
            }
        }
    }
}
