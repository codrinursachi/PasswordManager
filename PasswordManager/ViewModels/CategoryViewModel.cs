﻿using PasswordManager.DTO;
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
using System.Windows.Input;
using System.Windows.Threading;

namespace PasswordManager.ViewModels
{
    class CategoryViewModel : ViewModelBase, IStopTimer
    {
        private CategoryNodeModel filter;
        private DispatcherTimer timer;

        public CategoryViewModel()
        {
            PasswordRepository passwordRepository = new();
            var rootNode = BuildTree(passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString(), App.Current.Properties["SelectedDb"].ToString() + ".json").Select(p => p.CategoryPath).Distinct().Where(p => p != null).ToList());
            Categories.Add(rootNode);
            SetupTimer();
        }

        public ObservableCollection<CategoryNodeModel> Categories { get; set; } = new();
        public ObservableCollection<PasswordToShowDTO> Passwords { get; } = new();
        public CategoryNodeModel Filter
        {
            get => filter;
            set
            {
                filter = value;
                OnPropertyChanged(nameof(Filter));
                App.Current.Properties["ShouldRefresh"] = true;
            }
        }

        public void Stop()
        {
            timer.Stop();
        }

        private void SetupTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            FilterPwd();
        }

        private void FilterPwd()
        {
            if (!(bool)App.Current.Properties["ShouldRefresh"])
            {
                return;
            }

            App.Current.Properties["ShouldRefresh"] = false;
            Passwords.Clear();
            PasswordRepository passwordRepository = new();
            if (Filter == null || Filter.Parent == null)
            {
                foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString(), App.Current.Properties["SelectedDb"].ToString() + ".json").Select(p => p.ToPasswordToShow()))
                {
                    Passwords.Add(password);
                }

                return;
            }

            string filter = string.Empty;
            List<string> path = new();
            var node = Filter;
            path.Add(node.Name);
            while (node.Parent.Name != "Categories")
            {
                node = node.Parent;
                path.Add(node.Name + "\\");
            }

            path.Reverse();
            foreach (var item in path)
            {
                filter += item;
            }

            foreach (var password in passwordRepository.GetAllPasswords(App.Current.Properties["pass"].ToString(), App.Current.Properties["SelectedDb"].ToString()+".json").Where(p => p.CategoryPath != null && p.CategoryPath.StartsWith(filter) && (string.IsNullOrEmpty(p.CategoryPath[filter.Length..]) || p.CategoryPath[filter.Length] == '\\')).Select(p => p.ToPasswordToShow()))
            {
                Passwords.Add(password);
            }
        }

        private CategoryNodeModel BuildTree(List<string> paths)
        {
            var root = new CategoryNodeModel { Name = "Categories" };
            foreach (var path in paths)
            {
                var parts = path.Split('\\');
                var current = root;
                foreach (var part in parts)
                {
                    var child = current.Children.FirstOrDefault(p => p.Name == part);
                    if (child == null)
                    {
                        child = new CategoryNodeModel { Name = part, Parent = current };
                        current.Children.Add(child);
                    }

                    current = child;
                }
            }

            return root;
        }

    }
}

