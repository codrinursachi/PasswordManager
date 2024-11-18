using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using PasswordManager.ViewModels;
using PasswordManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PasswordManager.Services
{
    public partial class NavigationToChildViewService : ObservableObject, INavigationToChildViewService
    {
        [ObservableProperty]
        public UserControl childView;
        Func<Type, UserControl> childViewFactory;
        public NavigationToChildViewService(Func<Type, UserControl> childViewFactory)
        {
            this.childViewFactory = childViewFactory;
        }
        public void SetChildView(ObservableObject childViewModel)
        {
            if (childViewModel is AllPasswordsViewModel)
            {
                ChildView = childViewFactory.Invoke(typeof(AllPasswordsView));
            }
            if (childViewModel is FavoritesViewModel)
            {
                ChildView = childViewFactory.Invoke(typeof(FavoritesView));
            }
            if (childViewModel is CategoryViewModel)
            {
                ChildView = childViewFactory.Invoke(typeof(CategoryView));
            }
        }
    }
}
