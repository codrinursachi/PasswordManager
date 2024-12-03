namespace PasswordManager.Interfaces
{
    public interface IAutoLockerService
    {
        void OnActivity(object? sender, EventArgs e);
        void SetupTimer();
    }
}
