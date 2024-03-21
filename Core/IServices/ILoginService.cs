using Core.Models;

namespace Core.IServices
{
    public interface ILoginService
    {
        bool ValidateCredentials(string username, string password);
        string GenerateJwtToken(string username);
    }
}
