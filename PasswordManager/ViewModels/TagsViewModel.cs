using PasswordManager.DTO;
using PasswordManager.DTO.Extensions;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using System.Collections.ObjectModel;

namespace PasswordManager.ViewModels
{
    class TagsViewModel : ViewModelBase, IRefreshable
    {
        private List<string> filter = new();

        public TagsViewModel()
        {
            FilterPass();
        }

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

        public void FilterPass()
        {
            IPasswordRepository passwordRepository = new PasswordRepository();
            Passwords.Clear();

            if (Filter.Count == 0)
            {
                foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString(),App.Current.Properties["SelectedDb"].ToString() + ".json").Select(p => p.ToPasswordToShow()))
                {
                    Passwords.Add(password);
                }
            }
            else
            {
                foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString(), App.Current.Properties["SelectedDb"].ToString() + ".json").Select(p => p.ToPasswordToShow()).Where(p => p.Tags != null && p.Tags.Split().ToHashSet().IsSupersetOf(Filter)))
                {
                    Passwords.Add(password);
                }
            }
        }

        public void Refresh()
        {
            IPasswordRepository passwordRepository = new PasswordRepository();
            var tags = passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString(), App.Current.Properties["SelectedDb"].ToString() + ".json").Where(p => p.Tags != null).Select(p => p.Tags.Split());
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
        }
    }
}
