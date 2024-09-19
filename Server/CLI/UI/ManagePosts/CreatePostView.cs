using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView(IPostRepository postRepository) : IConsoleView
{
    private readonly IPostRepository _postRepository = postRepository;

    private async Task CreatePost()
    {
        
        Console.WriteLine("Please enter post title");
        var title = Console.ReadLine();

        if (title == null)
        {
            Console.WriteLine("Invalid title");
            return;
        }
        
        Console.WriteLine("Please enter post content");
        var content = Console.ReadLine();

        if (content == null)
        {
            Console.WriteLine("Invalid content");
            return;
        }
        
        Console.WriteLine("Enter users id");
        var userId = Console.ReadLine();
        
        if (userId == null || !int.TryParse(userId, out var userIdInt))
        {
            Console.WriteLine("Invalid user id");
            return;
        }
        
        var post = new Post
        {
            Title = title,
            Body = content,
            UserId = userIdInt
        };

        await _postRepository.AddAsync(post);
        Console.WriteLine("Post created successfully");
    }
    
    
    public Task ShowConsoleContent()
    {
        return CreatePost();
    }
}