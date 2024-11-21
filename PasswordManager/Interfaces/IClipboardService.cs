using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PasswordManager.Interfaces
{
    public interface IClipboardService
    {
        void CopyToClipboard(object value);
        void TimedCopyToClipboard(object value);
    }
}
