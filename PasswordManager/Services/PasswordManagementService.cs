using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<PasswordModel> GetAllPasswords()
        {
            return passwordRepository.GetAllPasswords();
        }

        public PasswordModel GetPasswordById(int id)
        {
            return passwordRepository.GetPasswordById(id);
        }

        public CategoryNodeModel GetPasswordsCategoryRoot()
        {
            var paths = passwordRepository.GetAllPasswords().Select(p => p.CategoryPath).Distinct().Where(p => p != null).ToList();
            var root = new CategoryNodeModel { Name = "Categories" };
            foreach (var path in paths)
            {
                var parts = path.Split('\\');
                var current = root;
                foreach (var part in parts)
                {
                    var child = current.Children.FirstOrDefault(p => p.Name == part);
                    if (child == null)
                    {
                        child = new CategoryNodeModel { Name = part, Parent = current };
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
