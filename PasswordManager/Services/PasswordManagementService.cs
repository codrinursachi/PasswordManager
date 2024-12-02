using Microsoft.EntityFrameworkCore;
using PasswordManager.Data;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Models.Extensions;

namespace PasswordManager.Services
{
    public class PasswordManagementService : IPasswordManagementService
    {
        private IDatabaseInfoProviderService databaseInfoProviderService;
        private IPathProviderService pathProviderService;
        private IPasswordEncryptionService passwordEncryptionService;
        private IDbContextPoolService dbContextPoolService;
        public PasswordManagementService(
            IDatabaseInfoProviderService databaseInfoProviderService,
            IPathProviderService pathProviderService,
            IPasswordEncryptionService passwordEncryptionService,
            IDbContextPoolService dbContextPoolService)
        {
            this.databaseInfoProviderService = databaseInfoProviderService;
            this.pathProviderService = pathProviderService;
            this.passwordEncryptionService = passwordEncryptionService;
            this.dbContextPoolService = dbContextPoolService;
        }

        public void Add(PasswordModel passwordModel)
        {
            var dbContext = dbContextPoolService.Acquire(databaseInfoProviderService.CurrentDatabase);
            passwordModel.Password = passwordEncryptionService.Encrypt(passwordModel.Password);
            dbContext.Passwords.Add(passwordModel);
            dbContext.SaveChanges();
            dbContextPoolService.Release(databaseInfoProviderService.CurrentDatabase, dbContext);
        }

        public void Edit(int id, PasswordModel newPasswordModel)
        {
            var dbContext = dbContextPoolService.Acquire(databaseInfoProviderService.CurrentDatabase);
            var oldPass = dbContext.Passwords.FirstOrDefault(p => p.Id == id);
            oldPass.Url = newPasswordModel.Url;
            oldPass.Password = passwordEncryptionService.Encrypt(newPasswordModel.Password);
            oldPass.Username = newPasswordModel.Username;
            oldPass.ExpirationDate = newPasswordModel.ExpirationDate;
            oldPass.CategoryPath = newPasswordModel.CategoryPath;
            oldPass.Favorite = newPasswordModel.Favorite;
            oldPass.Notes = newPasswordModel.Notes;
            oldPass.Tags = newPasswordModel.Tags;
            dbContext.SaveChanges();
            dbContextPoolService.Release(databaseInfoProviderService.CurrentDatabase, dbContext);

        }

        public List<PasswordToShowModel> GetAllPasswords()
        {
            List<PasswordToShowModel> result;
            var dbContext = dbContextPoolService.Acquire(databaseInfoProviderService.CurrentDatabase);
            result = dbContext.Passwords.Select(p => p.ToPasswordToShowModel()).ToList();
            dbContextPoolService.Release(databaseInfoProviderService.CurrentDatabase, dbContext);

            return result;
        }

        public List<PasswordToShowModel> GetFilteredPasswords(string filter)
        {
            var allPasswords = GetAllPasswords();
            if (string.IsNullOrEmpty(filter))
            {
                return allPasswords;
            }
            List<PasswordToShowModel> passwords = [];
            foreach (var password in allPasswords)
            {
                List<string> searchData = [];
                if (password.Username != null)
                {
                    searchData.Add(password.Username);
                }
                if (password.CategoryPath != null)
                {
                    searchData.AddRange(password.CategoryPath.Split('/'));
                }
                if (password.Notes != null)
                {
                    searchData.AddRange(password.Notes.Split());
                }
                if (password.Tags != null)
                {
                    searchData.AddRange(password.Tags.Split());
                }
                if (password.Url != null)
                {
                    searchData.Add(password.Url);
                }
                if (searchData.ToHashSet().IsSupersetOf(filter.Split()))
                {
                    passwords.Add(password);
                }
            }

            return passwords;
        }

        public PasswordModel GetPasswordById(int id)
        {
            var dbContext = dbContextPoolService.Acquire(databaseInfoProviderService.CurrentDatabase);
            var pass = dbContext.Passwords.FirstOrDefault(p => p.Id == id);
            dbContextPoolService.Release(databaseInfoProviderService.CurrentDatabase, dbContext);
            return new PasswordModel
            {
                Id = pass.Id,
                Url = pass.Url,
                Password = passwordEncryptionService.Decrypt(pass.Password),
                Username = pass.Username,
                ExpirationDate = pass.ExpirationDate,
                CategoryPath = pass.CategoryPath,
                Favorite = pass.Favorite,
                Notes = pass.Notes,
                Tags = pass.Tags
            };
        }

        public CategoryNodeModel GetPasswordsCategoryRoot()
        {
            List<string> paths;
            var dbContext = dbContextPoolService.Acquire(databaseInfoProviderService.CurrentDatabase);
            paths = dbContext.Passwords.AsParallel().Select(p => p.CategoryPath).Distinct().Where(p => !string.IsNullOrEmpty(p)).ToList();
            dbContextPoolService.Release(databaseInfoProviderService.CurrentDatabase, dbContext);

            var root = new CategoryNodeModel { Name = "Categories", Level = 0 };
            foreach (var path in paths)
            {
                var parts = path.Split('\\');
                var current = root;
                foreach (var part in parts)
                {
                    var child = current.Children.FirstOrDefault(p => p.Name == part);
                    if (child == null)
                    {
                        child = new CategoryNodeModel { Name = part, Parent = current, Level = current.Level + 1 };
                        current.Children.Add(child);
                    }

                    current = child;
                }
            }

            return root;
        }

        public void Remove(int id)
        {
            var dbContext = dbContextPoolService.Acquire(databaseInfoProviderService.CurrentDatabase);
            dbContext.Passwords.Remove(dbContext.Passwords.FirstOrDefault(p => p.Id == id));
            dbContext.SaveChanges();
            dbContextPoolService.Release(databaseInfoProviderService.CurrentDatabase, dbContext);
        }
    }
}
