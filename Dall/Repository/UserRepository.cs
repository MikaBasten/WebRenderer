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
        private readonly YourDbContext _dbContext;

        public UserRepository(YourDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges(); // Save changes to the database
        }

        public bool IsValidUser(string username, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            return user != null && user.PasswordHash == password; 
        }

        public User GetUserByUsername(string username)
        {
            User user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            return user;
        }

    }
}
