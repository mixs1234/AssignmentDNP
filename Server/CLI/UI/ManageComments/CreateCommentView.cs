using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView(ICommentRepository commentRepository) : IConsoleView
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    
    
    private async Task CreateComment()
    {
        Console.WriteLine("Please enter comment content");
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
        
        Console.WriteLine("Enter post id");
        var postId = Console.ReadLine();
        
        if (postId == null || !int.TryParse(postId, out var postIdInt))
        {
            Console.WriteLine("Invalid post id");
            return;
        }
        
        var comment = new Comment
        {
            Body = content,
            UserId = userIdInt,
            PostId = postIdInt
        };

        await _commentRepository.AddAsync(comment);
        Console.WriteLine("Comment created successfully");
        
    }
    
    
    public Task ShowConsoleContent()
    {
        return CreateComment();
    }

    
}