namespace ApiContracts;

public class UpdateCommentDTO
{
    public required string Body { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    
}