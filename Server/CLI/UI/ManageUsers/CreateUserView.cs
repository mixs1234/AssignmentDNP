using System.Runtime.InteropServices;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView(IUserRepository userRepository) : IConsoleView
{
    private readonly IUserRepository _userRepository = userRepository;

    private async Task CreateUser()
    {
        var isUserNameValid = false;
        var user = new User();
        
        while (!isUserNameValid)
        {
            Console.WriteLine("Please enter new username");
            var username = Console.ReadLine();
            
            var usernameExists = _userRepository.GetMany().Any(userInRepository => userInRepository.Name.Equals(username));

            if (usernameExists)
            {
                Console.WriteLine("Username already exists. Please try again.");
                continue; 
            }

            isUserNameValid = true;
            if (username != null) user.Name = username;
        }


        var isPasswordValid = false;
        while (!isPasswordValid)
        {
            Console.WriteLine("Please enter password");
            var password1 = Console.ReadLine();

            Console.WriteLine("repeat password");
            var password2 = Console.ReadLine();

            if (password1 != null && password2 != null && password1.Equals(password2))
            {
                isPasswordValid = true;
                user.Password = password1;
            }
            
            Console.WriteLine("The two passwords does not match");
        }

        await _userRepository.AddAsync(user);
        
        Console.WriteLine("User created successfully");
    }

    public Task ShowConsoleContent()
    {
        return CreateUser();
    }
}