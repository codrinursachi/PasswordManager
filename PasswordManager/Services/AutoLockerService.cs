using PasswordManager.Interfaces;
using System.Windows;
using System.Windows.Threading;

namespace PasswordManager.Services
{
    public class AutoLockerService : IAutoLockerService
    {
        private DispatcherTimer timer;
        public void OnActivity(object? sender, EventArgs e)
        {
            timer.Stop();
            timer.Start();
        }

        public void SetupTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(240);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Application.Current.Properties["timeout"] = true;
            Application.Current.Shutdown();
        }

    }
}
