using PasswordManager.Interfaces;
using PasswordManager.Views;
using System.Windows;

namespace PasswordManager.Services
{
    public class AppStartupService : IAppStartupService
    {
        private IProgramFoldersCreatorService programFoldersCreatorService;
        private IWindowProviderService windowProviderService;
        private IDatabaseInfoProviderService databaseInfoProviderService;
        public AppStartupService(
            IProgramFoldersCreatorService programFoldersCreatorService,
            IWindowProviderService windowProviderService,
            IDatabaseInfoProviderService databaseInfoProviderService)
        {
            this.programFoldersCreatorService = programFoldersCreatorService;
            this.windowProviderService = windowProviderService;
            this.databaseInfoProviderService = databaseInfoProviderService;
        }

        public void Start(bool startMinimized)
        {
            programFoldersCreatorService.CreateFoldersIfNecessary();
            var loginView = windowProviderService.ProvideWindow<LoginView>();
            loginView.Show();
            if (startMinimized)
            {
                loginView.WindowState = WindowState.Minimized;
            }
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded && databaseInfoProviderService.DBPass != null)
                {
                    var mainView = windowProviderService.ProvideWindow<MainView>();
                    mainView.Show();
                    loginView.Close();
                }
            };

            App.Current.Properties["timeout"] = false;
        }
    }
}
