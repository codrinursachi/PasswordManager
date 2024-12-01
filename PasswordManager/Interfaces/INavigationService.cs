using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.ViewModels;

namespace PasswordManager.Interfaces
{
    public interface INavigationService
    {
        ObservableObject CurrentView { get; set; }
        void NavigateTo<TViewModel>() where TViewModel : ObservableObject;
    }
}
