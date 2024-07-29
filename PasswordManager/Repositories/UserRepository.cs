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
                File.Create(fileName);
            }
        }

        public void Add(UserModel userModel)
        {
            if (GetByUsername(userModel.UserName) != null)
            {
                return;
            }
            List<UserModel> users = new List<UserModel>();
            try
            {
                string allUsers = File.ReadAllText(fileName);
                users = JsonSerializer.Deserialize<List<UserModel>>(allUsers)!;
            }
            catch
            {

            }

            users.Add(new UserModel { UserName = userModel.UserName, Password = SecretHasher.Hash(userModel.Password) });
            string newData = JsonSerializer.Serialize<List<UserModel>>(users);
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
            List<UserModel> users=null;
            try
            {
                users = JsonSerializer.Deserialize<List<UserModel>>(jsonString)!;
            }
            catch
            {

            }
            return users?.FirstOrDefault(u => u.UserName == username);
        }

        public void Remove(string username)
        {
            throw new NotImplementedException();
        }
    }
}
