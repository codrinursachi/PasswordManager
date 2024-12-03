using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using PasswordManager.Views;
using System.Windows.Controls;

namespace PasswordManager.Services
{
    public partial class NavigationService : INavigationService
    {
        public ObservableObject CurrentViewModel { get; set; }
        public ContentControl CurrentView { get; set; }
        private Func<Type, ObservableObject> viewModelFactory;
        private IUserControlProviderService userControlProviderService;
        private IRefreshService refreshService;

        public NavigationService(
            Func<Type, ObservableObject> viewModelFactory,
            IUserControlProviderService userControlProviderService,
            IRefreshService refreshService)
        {
            this.viewModelFactory = viewModelFactory;
            this.userControlProviderService = userControlProviderService;
            this.refreshService = refreshService;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ObservableObject
        {
            ObservableObject viewModel = viewModelFactory.Invoke(typeof(TViewModel));
            CurrentViewModel = viewModel;
            if (CurrentViewModel is AllPasswordsViewModel)
            {
                CurrentView.Content = userControlProviderService.ProvideUserControl<AllPasswordsView>();
            }
            if (CurrentViewModel is FavoritesViewModel)
            {
                CurrentView.Content = userControlProviderService.ProvideUserControl<FavoritesView>();
            }
            if (CurrentViewModel is CategoryViewModel)
            {
                CurrentView.Content = userControlProviderService.ProvideUserControl<CategoryView>();
            }

            refreshService.View = (IRefreshable)CurrentViewModel;
            refreshService.RefreshMain();
        }
    }
}
