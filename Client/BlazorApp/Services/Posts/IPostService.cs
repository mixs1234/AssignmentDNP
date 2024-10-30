using System.Collections.ObjectModel;
using ApiContracts;

namespace BlazorApp.Services.Posts;

public interface IPostService
{
    public Task<PostDTO> AddPostAsync(CreatePostDTO request);
    public Task UpdatePostAsync(int id, CreatePostDTO request);
    public Task DeletePostAsync(int id);
    public Task<PostDTO> GetPostAsync(int id);
    public Task<Collection<PostDTO>> GetPostsAsync();
}