using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView(IUserRepository userRepository) : IConsoleView
{

    private readonly IUserRepository _userRepository = userRepository;

    private Task ListUsers()
    {
        string output = "";
        foreach(var user in _userRepository.GetMany())
        {
            PrintUser(user);
        }
        return Task.CompletedTask;
    }

    private static void PrintUser(User user)
    {
        Console.WriteLine($"User:{user.Id} - with username : {user.Name} and password : {user.Password}");
    }


    public Task ShowConsoleContent()
    {
        return ListUsers();
    }
}