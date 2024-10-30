using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services.Users;

public class HttpUserService : IUserService
{
    private readonly HttpClient _client;

    public HttpUserService(HttpClient client)
    {
        this._client = client;
    }

    public async Task<UserDTO> AddUserAsync(CreateUserDTO request)
    {
        var httpResponse = await _client.PostAsJsonAsync("/api/Users", request);
        var response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<UserDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public Task UpdateUserAsync(int id, CreateUserDTO request)
    {
        throw new NotImplementedException();
    }
}