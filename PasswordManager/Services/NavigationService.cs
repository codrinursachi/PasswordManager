using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using PasswordManager.ViewModels;

namespace PasswordManager.Services
{
    public partial class NavigationService : INavigationService
    {
        public ObservableObject CurrentView { get; set; }
        private Func<Type, ObservableObject> viewModelFactory;
        private INavigationToChildViewService navigationToChildViewService;

        public NavigationService(
            INavigationToChildViewService navigationToChildViewService, 
            Func<Type, ObservableObject> viewModelFactory)
        {
            this.viewModelFactory = viewModelFactory;
            this.navigationToChildViewService = navigationToChildViewService;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ObservableObject
        {
            ObservableObject viewModel = viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
            navigationToChildViewService.SetChildView(viewModel);
        }
    }
}
