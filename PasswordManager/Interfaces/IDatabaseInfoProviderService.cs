namespace PasswordManager.Interfaces
{
    public interface IDatabaseInfoProviderService
    {
        string CurrentDatabase { get; set; }
        byte[] DBPass { get; set; }
    }
}
