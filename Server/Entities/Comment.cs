namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }
    public User? User { get; set; }
    public int UserId { get; set; }
    public Post? Post { get; set; }
    public int PostId { get; set; }
    
    private Comment() {}
    
    public Comment(string body, int userId, int postId)
    {
        Body = body;
        UserId = userId;
        PostId = postId;
    }
}