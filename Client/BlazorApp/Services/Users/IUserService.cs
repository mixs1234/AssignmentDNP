using ApiContracts;

namespace BlazorApp.Services.Users;

public interface IUserService
{
    public Task<UserDTO> AddUserAsync(CreateUserDTO request);
    public Task UpdateUserAsync(int id, CreateUserDTO request);
    
}