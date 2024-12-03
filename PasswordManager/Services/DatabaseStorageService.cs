using PasswordManager.Interfaces;
using System.IO;

namespace PasswordManager.Services
{
    public partial class DatabaseStorageService : IDatabaseStorageService
    {
        private string programDbPath;

        public DatabaseStorageService(IPathProviderService pathProviderService)
        {
            programDbPath = Path.Combine(pathProviderService.ProgramPath, "Databases");
            Refresh();
        }

        public List<string> Databases { get; set; } = [];

        public void Refresh()
        {
            Databases.Clear();
            foreach (var db in Directory.GetFiles(programDbPath))
            {
                Databases.Add(db[(programDbPath + "\\").Length..^".db".Length]);
            }
            if (Databases.Count == 0)
            {
                File.Create(Path.Combine(programDbPath, "default.db")).Close();
                Databases.Add("default");
            }
        }

        public void Add(string dbName)
        {
            File.Create(Path.Combine(programDbPath, dbName + ".db")).Close();
            Refresh();
        }

        public void Remove(string dbName)
        {
            File.Delete(Path.Combine(programDbPath, dbName + ".db"));
            Refresh();
        }
    }
}
