using AccManagment.API.Entities;
using AccManagment.API.Modules;

namespace AccManagment.API.Services;

public interface IUserInfoRepository
{
    public Task<IEnumerable<User>> GetUsersAsync();
    public Task AddUserAsync(User user);
    public Task<(bool success, string massage, string? token)> SignInAsync(UserForm userForm);
    public bool verifyUserToken(string token);
    public int GetIdByJWT(string token);
    public Task<bool> MakeUserTwoFactorable(User? user);
    public Task<User?> GetUserByIdAsync(int userId);
    public Task<bool> IsUserNameExists(string username);

    public Task DeleteAll();
}