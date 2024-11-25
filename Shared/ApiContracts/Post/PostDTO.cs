namespace ApiContracts;

public class PostDTO
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    public int UserId { get; set; }
    public UserDTO? User { get; set; }
    public List<CommentDTO>? Comments { get; set; }
}