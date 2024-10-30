using System.Collections.ObjectModel;
using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services.Comments;

public class HttpCommentService : ICommentService 
{
    private readonly HttpClient _client;
    
    public HttpCommentService(HttpClient httpClient)
    {
        _client = httpClient;
    }
    
    public async Task<CommentDTO> AddCommentAsync(CreateCommentDTO request, int postId)
    {
        var httpResponse = await _client.PostAsJsonAsync($"/api/Posts/{postId}/Comments", request);
        var response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<CommentDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}