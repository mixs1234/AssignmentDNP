namespace ApiContracts;

public class UpdatePostDTO
{
    public required string Title { get; set; }
    public required string Body { get; set; }
    public int UserId { get; set; }
}