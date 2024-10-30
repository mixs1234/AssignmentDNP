using ApiContracts;

namespace BlazorApp.Services.Users;

public interface IUserService
{
    public Task<UserDTO> AddUserAsync(CreateUserDTO request);
    
}