using PasswordManager.Interfaces;
using System.Windows;

namespace PasswordManager.Services
{
    public class WindowProviderService : IWindowProviderService
    {
        private Func<Type, Window> WindowFactory;

        public WindowProviderService(
            Func<Type, Window> WindowFactory)
        {
            this.WindowFactory = WindowFactory;
        }

        public Window ProvideWindow<TView>() where TView : Window
        {
            Window window = WindowFactory.Invoke(typeof(TView));
            return window;
        }
    }
}
