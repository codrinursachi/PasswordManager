using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;

namespace PasswordManager.Interfaces
{
    public interface INavigationToChildViewService
    {
        ContentControl ChildView { get; set; }
        void SetChildView(ObservableObject childView);
    }
}
