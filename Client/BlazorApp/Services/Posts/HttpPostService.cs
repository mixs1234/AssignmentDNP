using System.Collections.ObjectModel;
using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services.Posts;

public class HttpPostService : IPostService
{
    private readonly HttpClient _client;
    
    public HttpPostService(HttpClient client)
    {
        this._client = client;
    }
    
    public async Task<PostDTO> AddPostAsync(CreatePostDTO request)
    {
        var httpResponse = await _client.PostAsJsonAsync("/api/Posts", request);
        var response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<PostDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<Collection<PostDTO>> GetPostsAsync()
    {
        var posts = await _client.GetFromJsonAsync<Collection<PostDTO>>("api/Posts?includeComments=false");
        if (posts == null)
        {
            throw new Exception("Failed to retrieve posts.");
        }

        return posts;
    }

    public async Task<PostDTO> GetPostAsync(int id)
    {
        var httpResponse = await _client.GetAsync($"/api/Posts/{id}?includeComments=true");
        var response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<PostDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}