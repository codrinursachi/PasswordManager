namespace PasswordManager.Interfaces
{
    public interface ISecretHasherService
    {
        string Hash(char[] input, int iterations);
        bool Verify(char[] input, string hashString);
    }
}
