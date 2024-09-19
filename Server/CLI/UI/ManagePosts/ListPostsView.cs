using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView(IPostRepository postRepository) : IConsoleView
{
    private readonly IPostRepository _postRepository = postRepository;

    private async Task ListSingle(int id)
    {
        var post = await _postRepository.GetSingleAsync(id);
       
        if (post == null)
        {
            Console.WriteLine("Post not found");
            return;
        }
        
        Console.WriteLine($"Id: {post.Id}, Title: {post.Title}, Body: {post.Body}, UserId: {post.UserId}");
        
    }
    
    private async Task ListAll()
    {
        var posts = _postRepository.GetMany();
        foreach (var post in posts)
        {
            Console.WriteLine($"Id: {post.Id}, Title: {post.Title}, Body: {post.Body}, UserId: {post.UserId}");
        }
    }
    
    
    public Task ShowConsoleContent()
    {
        Console.WriteLine("List (A)ll posts or a (S)ingle post by id");
        var input = Console.ReadLine();
        
        if(input == null || input.Length != 1)
        {
            Console.WriteLine("Invalid input");
            return Task.CompletedTask;
        }

        switch (input.ToUpper())
        {
            case "A":
                return ListAll();
            case "S":
                Console.WriteLine("Enter post id");
                var id = Console.ReadLine();
                if (id == null || !int.TryParse(id, out var postId))
                {
                    Console.WriteLine("Invalid input");
                    return Task.CompletedTask;
                }
                return ListSingle(postId);
            default:
                Console.WriteLine("Invalid input");
                break;
        }
        return Task.CompletedTask;
    }
}