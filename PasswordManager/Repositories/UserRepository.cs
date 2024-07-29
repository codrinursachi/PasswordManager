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
            users.Add(userModel);
            string newData = JsonSerializer.Serialize(users);
            File.WriteAllText(fileName, newData);
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            throw new NotImplementedException();
        }

        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public UserModel GetByUsername(string username)
        {
            string jsonString = File.ReadAllText(fileName);
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
            users = JsonSerializer.Deserialize<List<UserModel>>(allUsers)!;

            return users;
        }
    }
}
