namespace PasswordManager.Interfaces
{
    public interface IUserRepository
    {
        public bool AuthenticateUser(char[] password);
    }
}
