using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebRenderer.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly string _secretKey;

        public AuthenticationController(IConfiguration configuration)
        {
            _secretKey = configuration["JwtSettings:SecretKey"];
        }

        [HttpPost("/authenticate-token")]
        public IActionResult AuthenticateToken([FromBody] string token)
        {
            try
            {
                // Validate the JWT token using the secret key
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // Token is valid
                return Ok(new { valid = true });
            }
            catch (Exception)
            {
                // Token is invalid or expired
                return Unauthorized(new { valid = false });
            }
        }
    }
}
