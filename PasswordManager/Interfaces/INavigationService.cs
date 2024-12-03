using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;

namespace PasswordManager.Interfaces
{
    public interface INavigationService
    {
        ObservableObject CurrentViewModel { get; set; }
        ContentControl CurrentView { get; set; }
        void NavigateTo<TViewModel>() where TViewModel : ObservableObject;
    }
}
