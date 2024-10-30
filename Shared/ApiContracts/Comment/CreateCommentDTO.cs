namespace ApiContracts;

public class CreateCommentDTO
{
    public required string Body { get; set; }
    public required int UserId { get; set; }
}