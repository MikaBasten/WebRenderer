using Core.IRepository;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly string _secretKey;

        public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, string secretKey)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _secretKey = secretKey;
        }

        public void Register(string username, string password)
        {
            // Username already exists
            if (_userRepository.GetUserByUsername(username) != null)
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Hash password
            string hashedPassword = HashPassword(password);

            // Create a new user object
            var newUser = new User
            {
                Username = username,
                PasswordHash = hashedPassword
            };

            // Save the user to the repository
            _userRepository.Add(newUser); // Implement Add method in UserRepository
        }

        public bool Authenticate(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            string hashedPassword = HashPassword(password);

            if (user != null)
            {
                
                // Verify password with the hashed password
                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, password);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    return true;
                }
            }
            return false; // Authentication failed
        }

        public string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Hash password method
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }
    }
}
