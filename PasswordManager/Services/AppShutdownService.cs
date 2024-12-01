using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
