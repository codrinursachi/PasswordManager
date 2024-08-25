using PasswordManager.DTO;
using PasswordManager.DTO.Extensions;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PasswordManager.ViewModels
{
    class TagsViewModel : ViewModelBase, IStopTimer
    {
        private List<string> _filter = new();
        private DispatcherTimer _timer;

        public TagsViewModel()
        {
            SetupTimer();
        }

        public ObservableCollection<PasswordToShowDTO> Passwords { get; set; } = new();
        public ObservableCollection<string> Tags
        {
            get; set;
        } = new();
        public List<string> Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                App.Current.Properties["ShouldRefresh"] = true;
            }
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void SetupTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            FilterPwd();
        }

        private void FilterPwd()
        {
            if (!(bool)App.Current.Properties["ShouldRefresh"])
            {
                return;
            }

            UpdateTags();
            App.Current.Properties["ShouldRefresh"] = false;
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

        private void UpdateTags()
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
