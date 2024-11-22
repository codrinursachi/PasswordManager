using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Utilities
{
    public class BackupCreator
    {
        private string appFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager");
        public void CreateBackupIfNecessary()
        {
            var pathToDb = Path.Combine(appFolder, "Databases");
            var pathToBackups = Path.Combine(appFolder, "Backups");
            if (!Directory.Exists(pathToBackups))
            {
                Directory.CreateDirectory(pathToBackups);
            }
            foreach (var db in (Directory.GetFiles(pathToDb)))
            {
                var (backupNecessary, oldestBackup) = CheckBackup(db[(pathToDb + "\\").Length..]);
                if (backupNecessary)
                {
                    if (oldestBackup != null)
                    {
                        File.Delete(oldestBackup);
                    }
                    CreateBackup(db);
                }
            }
        }

        private void CreateBackup(string db)
        {
            var pathToDb = Path.Combine(appFolder, "Databases");
            var pathToBackups = Path.Combine(appFolder, "Backups");
            File.Copy(db, pathToBackups + "\\" + db[(pathToDb + "\\").Length..] + "_" + DateTime.Now.ToString("dd.mm.yyyy") + ".bak");
        }

        private (bool backupNecessary, string oldestBackup) CheckBackup(string DbName)
        {
            int backupCount = 0;
            DateTime latestBackupTime = default;
            DateTime oldestBackupTime = DateTime.Now;
            string oldestBackup = string.Empty;

            var pathToBackups = Path.Combine(appFolder, "Backups");
            foreach (var db in Directory.GetFiles(pathToBackups))
            {
                if (db[(pathToBackups + "\\").Length..^"01.01.0001.bak".Length] == DbName + "_")
                {
                    backupCount++;
                    latestBackupTime = File.GetCreationTime(db) > latestBackupTime ? File.GetCreationTime(db) : latestBackupTime;
                    if (File.GetCreationTime(db) < oldestBackupTime)
                    {
                        oldestBackupTime = File.GetCreationTime(DbName);
                        oldestBackup = db;
                    }
                }
            }

            if (backupCount < 5)
            {
                oldestBackup = null;
            }

            return (latestBackupTime < DateTime.Now - TimeSpan.FromDays(7), oldestBackup);
        }
    }
}
