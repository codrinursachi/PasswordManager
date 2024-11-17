using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public partial class NavigationService : ObservableObject, INavigationService
    {
        [ObservableProperty]
        public ViewModel currentView;
        private Func<Type, ViewModel> viewModelFactory;

        public NavigationService(Func<Type,ViewModel> viewModelFactory)
        {
            this.viewModelFactory=viewModelFactory;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModel
        {
            ViewModel viewModel=viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
        }
    }
}
