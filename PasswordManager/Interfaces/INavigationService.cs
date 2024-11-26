using PasswordManager.ViewModels;

namespace PasswordManager.Interfaces
{
    public interface INavigationService
    {
        ViewModel CurrentView { get; set; }
        void NavigateTo<TViewModel>() where TViewModel : ViewModel;
    }
}
