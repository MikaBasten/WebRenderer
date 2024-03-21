using Core.IRepository;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public User Authenticate(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);

            if (user != null)
            {
                // Verify password with the hashed password
                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, password);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    return user; 
                }
            }

            return null; // Aut failed
        }

        // Hash password method
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }
    }
}
