using CommunityToolkit.Mvvm.ComponentModel;
using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public partial class DatabaseStorageService : ObservableObject, IDatabaseStorageService
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
                Databases.Add(db[(programDbPath + "\\").Length..^".json".Length]);
            }
            if (Databases.Count == 0)
            {
                File.Create(Path.Combine(programDbPath, "default.json")).Close();
                Databases.Add("default");
            }
        }

        public void Add(string dbName)
        {
            File.Create(Path.Combine(programDbPath, dbName+".json")).Close();
            Refresh();
        }

        public void Remove(string dbName)
        {
            File.Delete(Path.Combine(programDbPath, dbName + ".json"));
            Refresh();
        }
    }
}
