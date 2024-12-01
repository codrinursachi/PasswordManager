using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Models.Extensions;

namespace PasswordManager.Services
{
    public class PasswordManagementService : IPasswordManagementService
    {
        IPasswordRepository passwordRepository;
        public PasswordManagementService(IPasswordRepository passwordRepository)
        {
            this.passwordRepository = passwordRepository;
        }

        public void Add(PasswordModel passwordModel)
        {
            passwordRepository.Add(passwordModel);
        }

        public void Edit(int id, PasswordModel newPasswordModel)
        {
            passwordRepository.Edit(id, newPasswordModel);
        }

        public List<PasswordToShowModel> GetAllPasswords()
        {
            return passwordRepository.GetAllPasswords().Select(p => p.ToPasswordToShowModel()).ToList();
        }

        public List<PasswordToShowModel> GetFilteredPasswords(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return GetAllPasswords();
            }
            List<PasswordToShowModel> passwords = [];
            foreach (var password in GetAllPasswords())
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
            return passwordRepository.GetPasswordById(id);
        }

        public CategoryNodeModel GetPasswordsCategoryRoot()
        {
            var paths = passwordRepository.GetAllPasswords().Select(p => p.CategoryPath).Distinct().Where(p => !string.IsNullOrEmpty(p)).ToList();
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
            passwordRepository.Remove(id);
        }
    }
}
