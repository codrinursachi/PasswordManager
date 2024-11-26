namespace PasswordManager.Interfaces
{
    public interface IClipboardService
    {
        void CopyToClipboard(object value);
        void TimedCopyToClipboard(object value);
    }
}
