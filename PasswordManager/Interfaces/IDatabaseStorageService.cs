namespace PasswordManager.Interfaces
{
    public interface IDatabaseStorageService
    {
        public List<string> Databases { get; }
        public void Refresh();
        public void Add(string dbName);
        public void Remove(string dbName);
    }
}
