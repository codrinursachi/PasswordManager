using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace PasswordManager.Utilities
{
    public static class AutoLocker
    {
        private static DispatcherTimer timer;
        public static void OnActivity(object? sender, EventArgs e)
        {
            timer.Stop();
            timer.Start();
        }

        public static void SetupTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(240);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private static void TimerTick(object sender, EventArgs e)
        {
            App.Current.Properties["timeout"] = true;
            Application.Current.Shutdown();
        }

    }
}
