using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.CustomControls;
using PasswordManager.Interfaces;
using PasswordManager.Repositories;
using PasswordManager.Services;
using PasswordManager.ViewModels;
using PasswordManager.ViewModels.CustomControls;
using PasswordManager.ViewModels.Dialogs;
using PasswordManager.Views;
using PasswordManager.Views.Dialogs;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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
            services.AddSingleton<LoginView>();
            services.AddSingleton<MainView>();
            services.AddTransient<PasswordCreationView>();
            services.AddTransient<PasswordGeneratorView>();
            services.AddTransient<DataBaseManagerView>();

            services.AddSingleton<AllPasswordsView>();
            services.AddSingleton<FavoritesView>();
            services.AddSingleton<CategoryView>();
            services.AddTransient<PasswordSearch>();
            services.AddTransient<PasswordDataGrid>();
            services.AddTransient<PasswordModelEditor>();

            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<AllPasswordsViewModel>();
            services.AddSingleton<CategoryViewModel>();
            services.AddSingleton<FavoritesViewModel>();
            services.AddSingleton<PasswordDataGridViewModel>();
            services.AddTransient<PasswordGeneratorViewModel>();
            services.AddTransient<PasswordModelEditorViewModel>();
            services.AddTransient<DataBaseManagerViewModel>();

            services.AddSingleton<IDataContextProviderService, DataContextProviderService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<INavigationToChildViewService, NavigationToChildViewService>();
            services.AddSingleton<IModalDialogProviderService, ModalDialogProviderService>();
            services.AddSingleton<IModalDialogClosingService, ModalDialogClosingService>();
            services.AddKeyedSingleton<IMessenger, WeakReferenceMessenger>("GeneratedPassword");
            services.AddKeyedSingleton<IMessenger, WeakReferenceMessenger>("PasswordModel");
            services.AddKeyedSingleton<IMessenger, WeakReferenceMessenger>("PasswordList");
            services.AddSingleton<IDatabaseStorageService, DatabaseStorageService>();
            services.AddSingleton<IDatabaseInfoProviderService, DatabaseInfoProviderService>();
            services.AddSingleton<IUserControlProviderService, UserControlProviderService>();
            services.AddSingleton<IPasswordImporterService, PasswordImporterService>();
            services.AddSingleton<IPasswordRepository, PasswordRepository>();
            services.AddSingleton<IPasswordManagementService, PasswordManagementService>();
            services.AddSingleton<IClipboardService, ClipboardService>();

            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider => viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));
            services.AddSingleton<Func<Type, ObservableObject>>(serviceProvider => dataContextType => (ObservableObject)serviceProvider.GetRequiredService(dataContextType));
            services.AddSingleton<Func<Type, Window>>(serviceProvider => dialogType => (Window)serviceProvider.GetRequiredService(dialogType));
            services.AddSingleton<Func<Type, UserControl>>(serviceProvider => userControlType => (UserControl)serviceProvider.GetRequiredService(userControlType));


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
                if (loginView.IsVisible == false && loginView.IsLoaded)
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
