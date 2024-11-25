using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserRepository userRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetMany([FromQuery] string? nameContains = null)
    {
        var users = userRepository.GetMany();

        if (!string.IsNullOrEmpty(nameContains))
        {
            users = users.Where(user => user.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        }

        var userDtos = users.Select(user => new UserDTO()
        {
            Id = user.Id,
            Username = user.Name,
            Password = user.Password
        }).ToList();

        return Ok(userDtos);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDTO>> GetSingle(int id)
    {
        var user = await userRepository.GetSingleAsync(id);
        var userDto = new UserDTO
        {
            Id = user.Id,
            Username = user.Name,
            Password = user.Password,
        };
        
        return Ok(userDto);
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDTO>> Add(CreateUserDTO createUserDto)
    {
        var user = new User(createUserDto.Username, createUserDto.Password);
            
        var createdUser = await userRepository.AddAsync(user);
        var createdUserDto = new UserDTO
        {
            Id = createdUser.Id,
            Username = createdUser.Name,
            Password = createdUser.Password,
        };
        
        return Ok(createdUserDto);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDTO>> Update([FromRoute] int id,
        UpdateUserDTO updateUserDto)
    {
        var user = new User(updateUserDto.Username, updateUserDto.Password)
        {
            Id = id
        };
        
        await userRepository.UpdateAsync(user);
        
        return Ok(user);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await userRepository.DeleteAsync(id);
        
        return NoContent();
    }
    
    
}