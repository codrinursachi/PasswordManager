namespace PasswordManager.Interfaces
{
    public interface IPasswordDeletionService
    {
        int Id { get; set; }
        void Delete();
    }
}
