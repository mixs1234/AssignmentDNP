using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApp.Controller;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;
    
    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponse>> Login([FromBody] LoginUserRequest request)
    {
        var user = userRepository.GetMany().FirstOrDefault(user => user.Name == request.Username);
        if (user == null)
        {
            return Unauthorized();
        }
        
        if (user.Password != request.Password)
        {
            return Unauthorized();
        }
        
        return Ok(new LoginUserResponse
        {
            Id = user.Id,
            Username = user.Name
        });
    }
    
}