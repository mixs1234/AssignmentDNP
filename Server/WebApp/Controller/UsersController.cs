using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    
    private readonly IUserRepository _userRepository;
    
    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetMany([FromQuery] string? nameContains = null)
    {
        var users = _userRepository.GetMany();

        if (!string.IsNullOrEmpty(nameContains))
        {
            users = users.Where(user => user.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        }

        var userDtos = users.Select(user => new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Password = user.Password
        }).ToList();

        return Ok(userDtos);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingle(int id)
    {
        var user = await _userRepository.GetSingleAsync(id);
        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Password = user.Password,
        };
        
        return Ok(userDto);
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> Add(UserDto userDto)
    {
        var user = new User
        {
            Name = userDto.Name,
            Password = userDto.Password,
        };
        
        var createdUser = await _userRepository.AddAsync(user);
        var createdUserDto = new UserDto
        {
            Id = createdUser.Id,
            Name = createdUser.Name,
            Password = createdUser.Password,
        };
        
        return Ok(createdUserDto);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Update(int id, UserDto userDto)
    {
        var user = new User
        {
            Id = id,
            Name = userDto.Name,
            Password = userDto.Password,
        };
        
        await _userRepository.UpdateAsync(user);
        
        return Ok(userDto);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _userRepository.DeleteAsync(id);
        
        return NoContent();
    }
    
    
}