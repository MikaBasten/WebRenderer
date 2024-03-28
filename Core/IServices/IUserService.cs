using Core.Models;

namespace Core.IServices
{
    public interface IUserService
    {
        void Register(string username, string password);
        bool Authenticate(string username, string password);
        string HashPassword(string password);
        string GenerateJwtToken(string username);
    }
}
