using Core.IRepository;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _secretKey;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginService(IUserRepository userRepository, string secretKey, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _secretKey = secretKey;
            _passwordHasher = passwordHasher;
        }

        public bool ValidateCredentials(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null)
                return false;

            // Verifypassword with the hashed password
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, password);

            //password verification was successful
            return passwordVerificationResult == PasswordVerificationResult.Success;
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
    }
}
