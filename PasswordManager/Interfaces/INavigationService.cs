using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Interfaces
{
    public interface INavigationService
    {
        ViewModel CurrentView { get; }
        void NavigateTo<TViewModel>() where TViewModel:ViewModel;
    }
}
