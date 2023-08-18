using AccManagment.API.Entities;
using AccManagment.API.Modules;
using AccManagment.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccManagment.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RegisterController : ControllerBase
{
    
    private readonly IUserInfoRepository _repository;
    private readonly ILogger<RegisterController> _logger;
    private readonly AuthenticatorApi _authenticatorApi;
    private readonly IMapper _mapper;
    

    public RegisterController (IUserInfoRepository repository, ILogger<RegisterController> logger,
        AuthenticatorApi authenticatorApi, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _authenticatorApi = authenticatorApi;
        _mapper = mapper;
    }
    
    
    [HttpPost("AddUser")]
    public async Task<ActionResult> AddUser(UserRegistration userForm)
    {
        if (string.IsNullOrEmpty(userForm.Username) || string.IsNullOrEmpty(userForm.Password))
        {
            return BadRequest("Invalid user data provided.");
        }
        try
        {
            await _repository.AddUserAsync(_mapper.Map<User>(userForm));
            return Ok(new { message = "User registered successfully" });
        }
        catch (DbUpdateException ex)
        {
            return Conflict(new { message = "Username already exists.", username = userForm.Username });
        }
        
    }

    
    [HttpPost("LogIn")]
    public async Task<ActionResult> LogIn(UserForm userForm)
    {
        var (success,massage,token) = await _repository.SignInAsync(userForm);
        if (!success)
        {
            return Unauthorized(new { message = massage });
        }
        return Ok(new {token = token , message = massage});
    }

    

    [HttpPut("Authenticator")]
    public async Task<ActionResult<string>> Authenticator(string token){
        if (!_repository.verifyUserToken(token))
        {
            return Unauthorized("Authorization failed");
        }

        int userId = _repository.GetIdByJWT(token);
        User? user = await _repository.GetUserByIdAsync(userId);
        
        
        if (await _repository.MakeUserTwoFactorable(user))
        {
            return await _authenticatorApi.PairWithGoogleAuthenticatorAsync(user.SecretCode);            
        }
        
        return StatusCode(500, 
            "An error occurred while enabling two-factor authentication. The user already has two-factor authentication. ");
    }
    

    [HttpPost("Validate")]
    public async Task<ActionResult> Validate( string token, string userEnteredCode)
    {
        if (!_repository.verifyUserToken(token))
        {
            return Unauthorized("Authorization failed");
        }
        int userId = _repository.GetIdByJWT(token);
        
        User user = await _repository.GetUserByIdAsync(userId) ?? throw new InvalidOperationException();
        
        if (await _authenticatorApi.ValidateGoogleAuthenticatorCodeAsync(userEnteredCode, user.SecretCode ?? throw new InvalidOperationException()))
        {
            return Ok("Code is valid. You have signed in");
        }

        return BadRequest("The code is not valid try again");
    }

    [HttpPost("getIdByToken")]
    public ActionResult<int> GetIdByToken(string token) => _repository.GetIdByJWT(token);


    [HttpDelete("DeleteAll")]
    public async Task DeleteAll()
    {
        await _repository.DeleteAll();
    }
    
    

}