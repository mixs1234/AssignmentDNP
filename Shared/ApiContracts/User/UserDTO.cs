namespace ApiContracts;

public class UserDTO
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}