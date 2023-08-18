using AccManagment.API.DbContecsts;
using AccManagment.API.Entities;
using AccManagment.API.Modules;
using Microsoft.EntityFrameworkCore;
using OtpNet;

namespace AccManagment.API.Services;

public class UserInfoRepository : IUserInfoRepository
{
    private readonly UserInfoContext _context;
    private readonly IHashedPassword _hashedPasswordService;
    private readonly ITokenService _tokenService;
    
    public UserInfoRepository(UserInfoContext context, IHashedPassword hashedPasswordService, ITokenService tokenService)
    {
        _context = context;
        _hashedPasswordService = hashedPasswordService;
        _tokenService = tokenService;
    }

    public async Task<IEnumerable<User>> GetUsersAsync() 
        => await _context.Users.ToListAsync();

    public async Task<User?> GetUserByIdAsync(int userId)
        => await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
    
    public async Task<bool> IsUserNameExists(string username) 
        => await _context.Users.AnyAsync(user => user.Username == username);

    public int GetIdByJWT(string token)
        => _tokenService.ExtractUserIdFromToken(token);
    
    public async Task AddUserAsync(User user)
    { 
        user.Password = _hashedPasswordService.HashPassword(user.Password);
        
        user.Token = _tokenService.GenerateToken(user);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    
   
    public async Task<(bool success, string massage, string? token)> SignInAsync(UserForm userForm)
    {
        var query = _context.Users as IQueryable<User>;
       
        var user = await query.FirstOrDefaultAsync(x => userForm.Username == x.Username);
        if (user==null)
        {
            return (false,"Username Not Found", null);
        }

        if (user.Has2Fac)
        {
            return (false, "Need two factor authentication", null);
        }

        bool isCorrectPassword = _hashedPasswordService.VerifyPassword(
            userForm.Password, user.Password);
        if (!isCorrectPassword)
        {
            return (false,"Password Is Not Correct", null);
        }
        
        
        
        string token = _tokenService.GenerateToken(user); // Generate the token

        user.Token = token;
        await _context.SaveChangesAsync();
        return (true,"signin successfully" ,user.Token) ;
    }

    

    public async Task<bool> MakeUserTwoFactorable(User? user)
    {
        var secret = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));
        if (user.Has2Fac == true)
        {
            return false;
        }
        
        user.Has2Fac = true;
        user.SecretCode = secret;
        await _context.SaveChangesAsync();
        return true;

    }

    public bool verifyUserToken(string token)
    {
        if (_tokenService.VerifyToken(token))
        {
            return true;
        }

        return false;
    }

    public async Task DeleteAll()
    {
        _context.RemoveRange(_context.Users);
        await _context.SaveChangesAsync();
    }
}