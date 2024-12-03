namespace PasswordManager.Interfaces
{
    public interface IUserRepository
    {
        bool AuthenticateUser(char[] password);
    }
}
