using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PasswordManager.Services
{
    class ClipboardService : IClipboardService
    {
        private object tempObject;
        private DispatcherTimer timer;

        public void CopyToClipboard(object value)
        {
            Clipboard.SetDataObject(value);
        }

        public void TimedCopyToClipboard(object value)
        {
            tempObject = value;
            Clipboard.SetDataObject(tempObject);
            SetupTimer();
        }

        private void SetupTimer()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
            };
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (string.Equals(Clipboard.GetText(),tempObject))
            {
                Clipboard.Clear();
            }

            timer.Stop();
        }
    }
}
