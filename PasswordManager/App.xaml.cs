using PasswordManager.Views;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            var loginView = new LoginView();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {
                    var mainView = new MainView();
                    mainView.Show();
                    loginView.Close();
                }
            };

            App.Current.Properties["ShouldRefresh"] = false;
            App.Current.Properties["timeout"] = false;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if ((bool)App.Current.Properties["timeout"])
            {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            }
        }
    }

}
