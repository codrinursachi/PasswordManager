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
        [ObservableProperty]
        private List<string> databases = [];
        public DatabaseStorageService()
        {
            Refresh();
        }
        public void Refresh()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Databases.Clear();
            foreach (var db in Directory.GetFiles(path))
            {
                Databases.Add(db[(path + "\\").Length..^".json".Length]);
            }
            if (Databases.Count == 0)
            {
                File.Create(Path.Combine(path, "default.json")).Close();
                Databases.Add("default");
            }
        }
    }
}
