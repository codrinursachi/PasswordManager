using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using PasswordManager.Views;
using System.Windows.Controls;

namespace PasswordManager.Services
{
    public partial class NavigationService : INavigationService
    {
        private IUserControlProviderService userControlProviderService;
        private IRefreshService refreshService;

        public NavigationService(
            IUserControlProviderService userControlProviderService,
            IRefreshService refreshService)
        {
            this.userControlProviderService = userControlProviderService;
            this.refreshService = refreshService;
        }

        public ContentControl CurrentView { get; set; }
        public ObservableObject CurrentViewModel =>(ObservableObject)((UserControl)CurrentView.Content).DataContext;

        public void NavigateTo<TViewModel>() where TViewModel : INavigationAware
        {
            UserControl view = (UserControl)CurrentView.Content;
            if(view != null)
            {
                ((INavigationAware)view.DataContext).OnNavigatedFrom();
            }
            if (typeof(TViewModel) == typeof(AllPasswordsViewModel))
            {
                view = userControlProviderService.ProvideUserControl<AllPasswordsView>();
            }
            else if (typeof(TViewModel) == typeof(FavoritesViewModel))
            {
                view = userControlProviderService.ProvideUserControl<FavoritesView>();
            }
            else if (typeof(TViewModel) == typeof(CategoryViewModel))
            {
                view = userControlProviderService.ProvideUserControl<CategoryView>();
            }

            CurrentView.Content = view;
            ((INavigationAware)view.DataContext).OnNavigatedTo();
            refreshService.View = (IRefreshable)view.DataContext;
            refreshService.RefreshMain();
            refreshService.RefreshPasswords();
        }
    }
}
