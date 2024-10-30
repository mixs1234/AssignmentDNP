using System.Collections.ObjectModel;
using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services.Posts;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;
    
    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task<PostDTO> AddPostAsync(CreatePostDTO request)
    {
        var httpResponse = await client.PostAsJsonAsync("/api/Posts", request);
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

    public Task UpdatePostAsync(int id, CreatePostDTO request)
    {
        throw new NotImplementedException();
    }

    public Task DeletePostAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<PostDTO> GetPostAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Collection<PostDTO>> GetPostsAsync()
    {
        var posts = await client.GetFromJsonAsync<Collection<PostDTO>>("api/Posts?includeComments=false");
        if (posts == null)
        {
            throw new Exception("Failed to retrieve posts.");
        }

        return posts;
    }
}