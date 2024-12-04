using PasswordManager.Interfaces;
using System.IO;

namespace PasswordManager.Services
{
    public class BackupManagementService : IBackupManagementService
    {
        private string programPath;
        public BackupManagementService(
            IPathProviderService pathProviderService)
        {
            programPath = pathProviderService.ProgramPath;
        }
        public void CreateBackupIfNecessary()
        {
            var pathToDb = Path.Combine(programPath, "Databases");
            var pathToBackups = Path.Combine(programPath, "Backups");
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
            var pathToDb = Path.Combine(programPath, "Databases");
            var pathToBackups = Path.Combine(programPath, "Backups");
            File.Copy(db, pathToBackups + "\\" + db[(pathToDb + "\\").Length..] + "_" + DateTime.Now.ToString("dd.MM.yyyy") + ".bak");
        }

        private (bool backupNecessary, string oldestBackup) CheckBackup(string DbName)
        {
            int backupCount = 0;
            DateTime latestBackupTime = default;
            DateTime oldestBackupTime = DateTime.Now;
            string oldestBackup = string.Empty;

            var pathToBackups = Path.Combine(programPath, "Backups");
            foreach (var db in Directory.GetFiles(pathToBackups))
            {
                if (Path.GetFileName(db)[..^"_01.01.0001.bak".Length] == DbName)
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
