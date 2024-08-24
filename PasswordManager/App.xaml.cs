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
            if (e.Args.Length > 0 && e.Args[0] == "--start-minimized")
            {
                loginView.WindowState = WindowState.Minimized;
            }
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded&& Thread.CurrentPrincipal!=null)
                {
                    var mainView = new MainView();
                    mainView.Show();
                    loginView.Close();
                }
            };

            App.Current.Properties["ShouldRefresh"] = true;
            App.Current.Properties["timeout"] = false;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if ((bool)App.Current.Properties["timeout"])
            {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName, "--start-minimized");
            }
        }
    }

}
