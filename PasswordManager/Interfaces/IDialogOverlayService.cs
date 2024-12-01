using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordManager.Interfaces
{
    public interface IDialogOverlayService
    {
        DialogOverlay MainViewOverlay { get; set; }
        DialogOverlay PasswordEditorOverlay { get; set; }
        void Show<TViewModel>() where TViewModel : ObservableObject;
        void Close();
    }
}
