namespace PasswordManager.Interfaces
{
    public interface IPasswordPair
    {
        char[] PasswordAsCharArray { get; set; }
        string Password { get; set; }
    }
}
