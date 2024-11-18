using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PasswordManager.Interfaces
{
    public interface INavigationToChildViewService
    {
        UserControl ChildView { get; set; }
        void SetChildView(ObservableObject childView);
    }
}
