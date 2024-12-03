namespace PasswordManager.Interfaces
{
    public interface IPasswordEncryptionService
    {
        char[] Encrypt(char[] input);
        char[] Decrypt(char[] input);
    }
}
