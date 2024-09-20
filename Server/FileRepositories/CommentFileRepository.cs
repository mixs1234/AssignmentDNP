using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    
    private const string FilePath = "Data/comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(FilePath))
        {
            File.WriteAllText(FilePath, "[]");
        }
    }
    
    private static async Task<List<Comment>> SavedComments()
    {
        var commentsJson = await File.ReadAllTextAsync(FilePath);
        return JsonSerializer.Deserialize<List<Comment>>(commentsJson)!;
    }
    
    private static async Task SaveComments(List<Comment> comments)
    {
        var commentsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(FilePath, commentsJson);
    }
    
    public async Task<Comment> AddAsync(Comment comment)
    {
        var commentsSaved = await SavedComments();
        
        var maxId = commentsSaved.Count != 0 ? commentsSaved.Max(x => x.Id) : 0;
        comment.Id = maxId + 1;
        
        commentsSaved.Add(comment);

        await SaveComments(commentsSaved);

        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        var commentsSaved = await SavedComments();
        
        var existingComment = commentsSaved.SingleOrDefault(x => x.Id == comment.Id);
        
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}' not found");
        }
        
        commentsSaved.Remove(existingComment);
        
        commentsSaved.Add(comment);
        
        await SaveComments(commentsSaved);
    }

    public async Task DeleteAsync(int id)
    {
        var commentsSaved = await SavedComments();
        
        var commentToRemove = commentsSaved.SingleOrDefault(x => x.Id == id);
        
        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }
        
        commentsSaved.Remove(commentToRemove);
        
        await SaveComments(commentsSaved);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        var commentsSaved = await SavedComments();
        
        var comment = commentsSaved.SingleOrDefault(x => x.Id == id);
        
        if (comment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }
        
        return comment;
    }

    public IQueryable<Comment> GetMany()
    {
        var commentsSaved = SavedComments().Result;
        return commentsSaved.AsQueryable();
    }
}