using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.CustomControls;
using PasswordManager.Data;
using PasswordManager.Interfaces;
using PasswordManager.Repositories;
using PasswordManager.Services;
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
            services.AddSingleton<LoginView>();
            services.AddSingleton<MainView>();

            services.AddTransient<PasswordGeneratorView>();
            services.AddTransient<DatabaseManagerView>();
            services.AddTransient<PasswordDeletionDialogView>();
            services.AddSingleton<AllPasswordsView>();
            services.AddSingleton<FavoritesView>();
            services.AddSingleton<CategoryView>();
            services.AddTransient<PasswordSearch>();
            services.AddTransient<PasswordDataGrid>();
            services.AddTransient<PasswordModelEditor>();
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
            services.AddSingleton<IModalDialogClosingService, ModalDialogClosingService>();
            services.AddKeyedSingleton<IMessenger, WeakReferenceMessenger>("GeneratedPassword");
            services.AddKeyedSingleton<IMessenger, WeakReferenceMessenger>("PasswordModel");
            services.AddKeyedSingleton<IMessenger, WeakReferenceMessenger>("PasswordList");
            services.AddSingleton<IDatabaseStorageService, DatabaseStorageService>();
            services.AddSingleton<IDatabaseInfoProviderService, DatabaseInfoProviderService>();
            services.AddSingleton<IUserControlProviderService, UserControlProviderService>();
            services.AddSingleton<IPasswordImporterService, PasswordImporterService>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IPasswordManagementService, PasswordManagementService>();
            services.AddSingleton<IClipboardService, ClipboardService>();
            services.AddSingleton<IPathProviderService, PathProviderService>();
            services.AddSingleton<IBackupManagementService, BackupManagementService>();
            services.AddSingleton<IProgramFoldersCreatorService, ProgramFoldersCreatorService>();
            services.AddSingleton<IModalDialogResultProviderService, ModalDialogResultProviderService>();
            services.AddSingleton<IAutoLockerService, AutoLockerService>();
            services.AddSingleton<ISecretHasherService, SecretHasherService>();
            services.AddSingleton<IDialogOverlayService, DialogOverlayService>();
            services.AddSingleton<IPasswordDeletionService, PasswordDeletionService>();
            services.AddSingleton<IRefreshService, RefreshService>();
            services.AddSingleton<IPasswordEncryptionService, PasswordEncryptionService>();
            services.AddSingleton<IDbContextPoolService, DbContextPoolService>();

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
