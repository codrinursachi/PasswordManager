using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Interfaces;
using PasswordManager.Services;
using PasswordManager.ViewModels;
using PasswordManager.ViewModels.CustomControls;
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
        private ServiceProvider serviceProvider;
        public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<LoginView>(serviceProvider => new LoginView()
            {
                DataContext=serviceProvider.GetRequiredService<LoginViewModel>()
            });
            services.AddSingleton<MainView>(serviceProvider => new MainView()
            {
                DataContext = serviceProvider.GetRequiredService<MainViewModel>()
            });

            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<MainViewModel>(serviceProvider => new MainViewModel(((IPasswordSettable)serviceProvider.GetRequiredService<LoginView>().DataContext).DBPass, serviceProvider.GetRequiredService<INavigationService>()));
            services.AddSingleton<AllPasswordsViewModel>();
            services.AddSingleton<CategoryViewModel>();
            services.AddSingleton<FavoritesViewModel>();
            services.AddSingleton<PasswordGeneratorViewModel>();
            services.AddSingleton<PasswordModelEditorViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider => viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));

            serviceProvider = services.BuildServiceProvider();
        }
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            var loginView = serviceProvider.GetRequiredService<LoginView>();
            loginView.Show();
            if (e.Args.Length > 0 && e.Args[0] == "--start-minimized")
            {
                loginView.WindowState = WindowState.Minimized;
            }
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded && (((IPasswordSettable)loginView.DataContext).DBPass != null))
                {
                    var mainView = serviceProvider.GetRequiredService<MainView>();
                    mainView.Show();
                    loginView.Close();
                }
            };

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
