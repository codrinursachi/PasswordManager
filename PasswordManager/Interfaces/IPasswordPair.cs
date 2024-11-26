namespace PasswordManager.Interfaces
{
    interface IPasswordPair
    {
        char[] PasswordAsCharArray { get; set; }
        string Password { get; set; }
    }
}
