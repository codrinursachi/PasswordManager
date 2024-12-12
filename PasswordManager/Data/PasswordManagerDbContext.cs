using Microsoft.EntityFrameworkCore;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.State;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Data
{
    public class PasswordManagerDbContext : DbContext
    {
        private IPathProviderService pathProviderService;
        private DatabaseState databaseInfoProviderService;
        public PasswordManagerDbContext(
            IPathProviderService pathProviderService,
            DatabaseState databaseInfoProviderService)
        {
            this.pathProviderService = pathProviderService;
            this.databaseInfoProviderService = databaseInfoProviderService;
            Database.EnsureCreated();
        }

        public DbSet<PasswordModel> Passwords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var pass = Encoding.UTF8.GetString(ProtectedData.Unprotect(databaseInfoProviderService.DBPass, null, DataProtectionScope.CurrentUser));
            var connectionString = $"Data Source={Path.Combine(pathProviderService.ProgramPath, "Databases", databaseInfoProviderService.CurrentDatabase)}.db;Password={pass}";
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}
