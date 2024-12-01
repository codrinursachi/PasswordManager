using PasswordManager.Interfaces;
using System.Diagnostics;

namespace PasswordManager.Services
{
    public class AppShutdownService : IAppShutdownService
    {
        public void Shutdown(bool hasTimedOut)
        {
            if (hasTimedOut)
            {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName, "--start-minimized");
            }
        }
    }
}
