namespace PasswordManager.Interfaces
{
    public interface IAppShutdownService
    {
        void Shutdown(bool hasTimedOut);
    }
}
