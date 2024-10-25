namespace ApiContracts;

public class CreateCommentDTO
{
    public required string Body { get; set; }
    public int UserId { get; set; }
}