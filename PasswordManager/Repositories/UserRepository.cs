using PasswordManager.Models;
using PasswordManager.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PasswordManager.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly string fileName = "UserLogin.json";

        public UserRepository()
        {
            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, JsonSerializer.Serialize(new List<UserModel>()));
            }
        }

        public void Add(UserModel userModel)
        {
            if (GetByUsername(userModel.UserName) != null)
            {
                return;
            }
            List<UserModel> users = GetUsersFromJsonFile();
            users.Add(new UserModel { UserName = userModel.UserName, Password = SecretHasher.Hash(userModel.Password) });
            string newData = JsonSerializer.Serialize(users);
            File.WriteAllText(fileName, newData);
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            var user = GetByUsername(credential.UserName);
            if (user == null)
            {
                return false;
            }

            return SecretHasher.Verify(credential.Password, new NetworkCredential("", user.Password).Password);
        }

        public void Edit(UserModel userModel)
        {
            var users = GetUsersFromJsonFile();
            var user = users.FirstOrDefault(u => u.UserName == userModel.UserName);
            user.Password = SecretHasher.Hash(userModel.Password);
            File.WriteAllText(fileName, JsonSerializer.Serialize(users));
        }

        public UserModel GetByUsername(string username)
        {
            List<UserModel> users = GetUsersFromJsonFile();
            return users.FirstOrDefault(u => u.UserName == username);
        }

        public void Remove(string username)
        {
            List<UserModel> users = GetUsersFromJsonFile();
            var user = users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return;
            }

            users.Remove(user);
            string newData = JsonSerializer.Serialize(users);
            File.WriteAllText(fileName, newData);
        }

        private List<UserModel> GetUsersFromJsonFile()
        {
            List<UserModel> users = [];
            string allUsers = File.ReadAllText(fileName);
            if (allUsers.Length != 0)
            {
                users = JsonSerializer.Deserialize<List<UserModel>>(allUsers)!;
            }

            return users;
        }
    }
}
