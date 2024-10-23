namespace ApiContracts
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
    }
    
    public class PostDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Body { get; set; }
        public int UserId { get; set; }
        public List<CommentDto>? Comments { get; set; }
    }
    
    public class CommentDto
    {
        public int Id { get; set; }
        public required string Body { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}