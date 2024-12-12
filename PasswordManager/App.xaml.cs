using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.CustomControls;
using PasswordManager.Data;
using PasswordManager.Interfaces;
using PasswordManager.Repositories;
using PasswordManager.Services;
using PasswordManager.State;
using PasswordManager.ViewModels;
using PasswordManager.ViewModels.Dialogs;
using PasswordManager.Views;
using PasswordManager.Views.Dialogs;
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
            services.AddSingleton<LoginView>(serviceProvider=>new LoginView()
            {
                DataContext=serviceProvider.GetRequiredService<LoginViewModel>()
            });
            services.AddSingleton<MainView>(serviceProvider=>new MainView(
                serviceProvider.GetRequiredService<IAutoLockerService>(),
                serviceProvider.GetRequiredService<IUserControlProviderService>(),
                serviceProvider.GetRequiredService<IDialogOverlayService>(),
                serviceProvider.GetRequiredService<INavigationService>()
                )
            {
                DataContext = serviceProvider.GetRequiredService<MainViewModel>()
            });

            services.AddTransient<PasswordGeneratorView>(serviceProvider=>new PasswordGeneratorView()
            {
                DataContext = serviceProvider.GetRequiredService<PasswordGeneratorViewModel>()
            });
            services.AddTransient<DatabaseManagerView>(serviceProvider=>new DatabaseManagerView()
            {
                DataContext = serviceProvider.GetRequiredService<DatabaseManagerViewModel>()
            });
            services.AddTransient<PasswordDeletionDialogView>(serviceProvider=>new PasswordDeletionDialogView()
            {
                DataContext = serviceProvider.GetRequiredService<PasswordDeletionDialogViewModel>()
            });
            services.AddSingleton<AllPasswordsView>(serviceProvider=>new AllPasswordsView(
                serviceProvider.GetRequiredService<IUserControlProviderService>())
            {
                DataContext = serviceProvider.GetRequiredService<AllPasswordsViewModel>()
            });
            services.AddSingleton<FavoritesView>(serviceProvider=>new FavoritesView(
                serviceProvider.GetRequiredService<IUserControlProviderService>())
            {
                DataContext = serviceProvider.GetRequiredService<FavoritesViewModel>()
            });
            services.AddSingleton<CategoryView>(serviceProvider=>new CategoryView(
                serviceProvider.GetRequiredService<IUserControlProviderService>())
            {
                DataContext = serviceProvider.GetRequiredService<CategoryViewModel>()
            });
            services.AddTransient<PasswordSearch>();
            services.AddTransient<PasswordDataGrid>(serviceProvider=>new PasswordDataGrid(
                serviceProvider.GetRequiredService<IUserControlProviderService>(),
                serviceProvider.GetRequiredService<IPasswordManagementService>())
            {
                DataContext = serviceProvider.GetRequiredService<PasswordDataGridViewModel>()
            });
            services.AddTransient<PasswordModelEditor>(serviceProvider=>new PasswordModelEditor(
                serviceProvider.GetRequiredService<IUserControlProviderService>(),
                serviceProvider.GetRequiredService<IDialogOverlayService>())
            {
                DataContext = serviceProvider.GetRequiredService<PasswordModelEditorViewModel>()
            });
            services.AddTransient<DialogOverlay>();

            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<AllPasswordsViewModel>();
            services.AddSingleton<CategoryViewModel>();
            services.AddSingleton<FavoritesViewModel>();
            services.AddTransient<PasswordDataGridViewModel>();
            services.AddTransient<PasswordGeneratorViewModel>();
            services.AddTransient<PasswordModelEditorViewModel>();
            services.AddTransient<DatabaseManagerViewModel>();
            services.AddTransient<PasswordDeletionDialogViewModel>();

            services.AddSingleton<IAppStartupService, AppStartupService>();
            services.AddSingleton<IAppShutdownService, AppShutdownService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IWindowProviderService, WindowProviderService>();
            services.AddKeyedSingleton<IMessenger, WeakReferenceMessenger>("GeneratedPassword");
            services.AddKeyedSingleton<IMessenger, WeakReferenceMessenger>("PasswordModel");
            services.AddKeyedSingleton<IMessenger, WeakReferenceMessenger>("PasswordList");
            services.AddSingleton<IDatabaseStorageService, DatabaseStorageService>();
            services.AddSingleton<IUserControlProviderService, UserControlProviderService>();
            services.AddSingleton<IPasswordImporterService, PasswordImporterService>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IPasswordManagementService, PasswordManagementService>();
            services.AddSingleton<IClipboardService, ClipboardService>();
            services.AddSingleton<IPathProviderService, PathProviderService>();
            services.AddSingleton<IBackupManagementService, BackupManagementService>();
            services.AddSingleton<IProgramFoldersCreatorService, ProgramFoldersCreatorService>();
            services.AddSingleton<IAutoLockerService, AutoLockerService>();
            services.AddSingleton<ISecretHasherService, SecretHasherService>();
            services.AddSingleton<IDialogOverlayService, DialogOverlayService>();
            services.AddSingleton<IPasswordDeletionService, PasswordDeletionService>();
            services.AddSingleton<IRefreshService, RefreshService>();
            services.AddSingleton<IPasswordEncryptionService, PasswordEncryptionService>();
            services.AddSingleton<IDbContextPoolService, DbContextPoolService>();

            services.AddSingleton<DatabaseState>();
            services.AddTransient<PasswordManagerDbContext>();

            services.AddSingleton<Func<Type, ObservableObject>>(serviceProvider => viewModelType => (ObservableObject)serviceProvider.GetRequiredService(viewModelType));
            services.AddSingleton<Func<Type, Window>>(serviceProvider => dialogType => (Window)serviceProvider.GetRequiredService(dialogType));
            services.AddSingleton<Func<Type, UserControl>>(serviceProvider => userControlType => (UserControl)serviceProvider.GetRequiredService(userControlType));
            services.AddSingleton<Func<PasswordManagerDbContext>>(serviceProvider => () => serviceProvider.GetRequiredService<PasswordManagerDbContext>());

            serviceProvider = services.BuildServiceProvider();
        }

        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            serviceProvider.GetRequiredService<IAppStartupService>().Start(e.Args.Length > 0 && e.Args[0] == "--start-minimized");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            serviceProvider.GetRequiredService<IAppShutdownService>().Shutdown((bool)App.Current.Properties["timeout"]);
        }
    }

}
