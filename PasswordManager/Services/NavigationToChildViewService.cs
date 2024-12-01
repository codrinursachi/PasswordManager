using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using PasswordManager.Views;
using System.Windows.Controls;

namespace PasswordManager.Services
{
    public partial class NavigationToChildViewService : INavigationToChildViewService
    {
        private IUserControlProviderService userControlProviderService;

        public NavigationToChildViewService(
            IUserControlProviderService userControlProviderService
            )
        {
            this.userControlProviderService = userControlProviderService;
        }

        public ContentControl ChildView { get; set; }

        public void SetChildView(ObservableObject childViewModel)
        {
            if (childViewModel is AllPasswordsViewModel)
            {
                ChildView.Content = userControlProviderService.ProvideUserControl<AllPasswordsView>();
            }
            if (childViewModel is FavoritesViewModel)
            {
                ChildView.Content = userControlProviderService.ProvideUserControl<FavoritesView>();
            }
            if (childViewModel is CategoryViewModel)
            {
                ChildView.Content = userControlProviderService.ProvideUserControl<CategoryView>();
            }
        }
    }
}
