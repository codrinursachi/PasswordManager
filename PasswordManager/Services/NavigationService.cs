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
    public partial class NavigationService : INavigationService
    {
        public ViewModel CurrentView { get; set; }
        private Func<Type, ViewModel> viewModelFactory;
        private INavigationToChildViewService navigationToChildViewService;

        public NavigationService(INavigationToChildViewService navigationToChildViewService, Func<Type,ViewModel> viewModelFactory)
        {
            this.viewModelFactory=viewModelFactory;
            this.navigationToChildViewService=navigationToChildViewService;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModel
        {
            ViewModel viewModel=viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
            navigationToChildViewService.SetChildView(viewModel);
        }
    }
}
