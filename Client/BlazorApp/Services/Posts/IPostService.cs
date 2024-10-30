using System.Collections.ObjectModel;
using ApiContracts;

namespace BlazorApp.Services.Posts;

public interface IPostService
{
    public Task<PostDTO> AddPostAsync(CreatePostDTO request);
    public Task<Collection<PostDTO>> GetPostsAsync();
    public Task<PostDTO> GetPostAsync(int id);
}