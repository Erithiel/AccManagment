using AccManagment.API.Entities;

namespace AccManagment.API.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    bool VerifyToken(string token);
    public int ExtractUserIdFromToken(string token);
}