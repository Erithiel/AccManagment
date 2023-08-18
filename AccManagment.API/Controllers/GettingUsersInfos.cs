using AccManagment.API.Entities;
using AccManagment.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccManagment.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class GettingUsersInfos : ControllerBase
{
    
    private readonly IUserInfoRepository _repository;

    public GettingUsersInfos(IUserInfoRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet("GetAllUsers")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        var users = await _repository.GetUsersAsync();
        return Ok(users);
    }
    [HttpGet("GetUserById/{userId}")]
    public async Task<ActionResult<User>> GetUserById(int  userId)
    {
        var user = await _repository.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        return user;
    }
    


}