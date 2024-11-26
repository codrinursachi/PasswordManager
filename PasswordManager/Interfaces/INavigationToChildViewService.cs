using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;

namespace PasswordManager.Interfaces
{
    public interface INavigationToChildViewService
    {
        UserControl ChildView { get; set; }
        void SetChildView(ObservableObject childView);
    }
}
