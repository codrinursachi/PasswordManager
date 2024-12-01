using CommunityToolkit.Mvvm.ComponentModel;

namespace PasswordManager.Interfaces
{
    public interface INavigationService
    {
        ObservableObject CurrentView { get; set; }
        void NavigateTo<TViewModel>() where TViewModel : ObservableObject;
    }
}
