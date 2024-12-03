using PasswordManager.Interfaces;

namespace PasswordManager.Services
{
    public class RefreshService : IRefreshService
    {
        public IRefreshable Main { get; set; }
        public IRefreshable View { get; set; }

        public void RefreshMain()
        {
            Main.Refresh();
        }

        public void RefreshPasswords()
        {
            View.Refresh();
        }
    }
}
