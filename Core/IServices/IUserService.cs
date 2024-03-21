using Core.Models;

namespace Core.IServices
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        string HashPassword(string password);
    }
}
