using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentsView(ICommentRepository commentRepository) : IConsoleView
{
    private readonly ICommentRepository _commentRepository = commentRepository;

    private async Task ListSingle(int id)
    {
        var comment = await _commentRepository.GetSingleAsync(id);
       
        if (comment == null)
        {
            Console.WriteLine("Comment not found");
            return;
        }
        
        Console.WriteLine($"Id: {comment.Id}, Body: {comment.Body}, UserId: {comment.UserId}, PostId: {comment.PostId}");
        
    }
    
    private async Task ListAll()
    {
        var comments = _commentRepository.GetMany();
        foreach (var comment in comments)
        {
            Console.WriteLine($"Id: {comment.Id}, Body: {comment.Body}, UserId: {comment.UserId}, PostId: {comment.PostId}");
        }
    }
    
    
    public Task ShowConsoleContent()
    {
        Console.WriteLine("List (A)ll comments or a (S)ingle comment by id");
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
                Console.WriteLine("Enter comment id");
                var id = Console.ReadLine();
                if (id == null || !int.TryParse(id, out var commentId))
                {
                    Console.WriteLine("Invalid input");
                    return Task.CompletedTask;
                }
                return ListSingle(commentId);
            default:
                Console.WriteLine("Invalid input");
                break;
        }

        return Task.CompletedTask;

    }
}