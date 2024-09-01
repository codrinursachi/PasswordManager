using PasswordManager.DTO;
using PasswordManager.DTO.Extensions;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using System.Collections.ObjectModel;

namespace PasswordManager.ViewModels
{
    public class TagsViewModel : ViewModelBase, IRefreshable,IDatabaseChangeable,IPasswordSettable
    {
        private List<string> filter = new();

        public ObservableCollection<PasswordToShowDTO> Passwords { get; set; } = new();
        public ObservableCollection<string> Tags{ get; set; } = new();
        public List<string> Filter
        {
            get => filter;
            set
            {
                filter = value;
                FilterPass();
            }
        }

        public string Database { get; set; }
        public byte[] DBPass { get; set; }

        private void FilterPass()
        {
            PasswordRepository passwordRepository = new(Database, DBPass);
            Passwords.Clear();

            if (Filter.Count == 0)
            {
                foreach (var password in passwordRepository.GetAllPasswords().Select(p => p.ToPasswordToShowDTO()))
                {
                    Passwords.Add(password);
                }
            }
            else
            {
                foreach (var password in passwordRepository.GetAllPasswords().Select(p => p.ToPasswordToShowDTO()).Where(p => p.Tags != null && p.Tags.Split().ToHashSet().IsSupersetOf(Filter)))
                {
                    Passwords.Add(password);
                }
            }
        }

        public void Refresh()
        {
            PasswordRepository passwordRepository = new(Database, DBPass);
            var tags = passwordRepository.GetAllPasswords().Where(p => p.Tags != null).Select(p => p.Tags.Split());
            HashSet<string> result = new();
            foreach (var itemTags in tags)
            {
                foreach (var tag in itemTags)
                {
                    result.Add(tag);
                }
            }

            if (!result.IsSupersetOf(Tags) || !Tags.ToHashSet().IsSupersetOf(result))
            {
                Tags.Clear();
                foreach (var tag in result)
                {
                    Tags.Add(tag);
                }
            }

            FilterPass();
        }
    }
}
