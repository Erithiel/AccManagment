using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccManagment.API.Entities;
using AccManagment.API.Modules;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace AccManagment.API.Services;

public class TokenService : ITokenService
{
    private string secretKey = "urYcV726LpLaL3TZTwESE1dQZONaXMPFjs0z8y+jJQY11jEdk9JZDVa1sXgweqCn";
    
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IOptions<JwtSettings> jwtSettings, ILogger<TokenService> logger)
    {
        _logger = logger;
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        _logger.LogInformation(key.ToString());
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier ,  user.Id.ToString())
                // Add additional claims as needed
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = "myApp",
            Issuer = "myApp"
            
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return tokenString;
    }
    
    public bool VerifyToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "myApp",
            ValidAudience = "myApp",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
        };

        try
        {
            SecurityToken validatedToken;
            tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            return true; 
        }
        catch
        {
            return false; 
        }
    }
    
    public int ExtractUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        if (tokenHandler.CanReadToken(token) && tokenHandler.ReadToken(token) is JwtSecurityToken jwtToken)
        {
            Claim? userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");
                
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
        }

        throw new ArgumentException("Invalid token or user ID claim not found.");
    }

}