using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    
    private const string FilePath = "Data/posts.json";
    
    public PostFileRepository()
    {
        if (!File.Exists(FilePath))
        {
            File.WriteAllText(FilePath, "[]");
        }
    }
    
    private static async Task<List<Post>> SavedPosts()
    {
        var postsJson = await File.ReadAllTextAsync(FilePath);
        return JsonSerializer.Deserialize<List<Post>>(postsJson)!;
    }
    
    private static async Task SavePosts(List<Post> posts)
    {
        var postsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(FilePath, postsJson);
    }
    
    public async Task<Post> AddAsync(Post post)
    {
        var postsSaved = await SavedPosts();
        
        var maxId = postsSaved.Count != 0 ? postsSaved.Max(x => x.Id) : 0;
        post.Id = maxId + 1;
        
        postsSaved.Add(post);

        await SavePosts(postsSaved);

        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        var postsSaved = await SavedPosts();
        
        var existingPost = postsSaved.SingleOrDefault(x => x.Id == post.Id);
        
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' not found");
        }
        
        postsSaved.Remove(existingPost);
        
        postsSaved.Add(post);
        
        await SavePosts(postsSaved);
    }

    public async Task DeleteAsync(int id)
    {
        var postsSaved = await SavedPosts();
        
        var postToRemove = postsSaved.SingleOrDefault(x => x.Id == id);
        
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }
        
        postsSaved.Remove(postToRemove);
        
        await SavePosts(postsSaved);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        var postsSaved = await SavedPosts();
        
        var post = postsSaved.SingleOrDefault(x => x.Id == id);
        
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }
        
        return post;
    }

    public IQueryable<Post> GetMany()
    {
        var postsSaved = SavedPosts().Result;
        return postsSaved.AsQueryable();
    }
}