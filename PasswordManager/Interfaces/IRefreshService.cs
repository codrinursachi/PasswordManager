namespace PasswordManager.Interfaces
{
    public interface IRefreshService
    {
        IRefreshable Main { get; set; }
        IRefreshable View { get; set; }
        void RefreshMain();
        void RefreshPasswords();
    }
}
