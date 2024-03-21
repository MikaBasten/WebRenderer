using Core.IServices;
using Core.Models;
using Core.IRepository;
using Dall.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dall.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users; // Assuming you have a list of users

        public UserRepository()
        {
            // Initialize your list of users
            _users = new List<User>
            {
                new User { Id = 1, Username = "exampleuser", PasswordHash = "hashedPassword" }
            };
        }

        public bool IsValidUser(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            return user != null && user.PasswordHash == password; // Adjust to use PasswordHash
        }

        public User GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }
    }
}
