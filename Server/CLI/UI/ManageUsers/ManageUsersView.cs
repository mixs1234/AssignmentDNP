using RepositoryContracts;

namespace CLI.UI.ManageUsers
{
    public class ManageUsersView(IUserRepository userRepository) : IConsoleView
    {
        private async Task DeleteUser(int id)
        {
            await userRepository.DeleteAsync(id);
        }

        private async Task UpdateUser(int id)
        {
            var userToUpdate = await userRepository.GetSingleAsync(id);
            var updateIsComplete = false;

            while (!updateIsComplete)
            {
                Console.WriteLine("Please enter a new username:");
                var newUserName = Console.ReadLine();

                if (newUserName != null && !await IsUsernameTakenAsync(newUserName))
                {
                    userToUpdate.Name = newUserName;
                }
                else
                {
                    Console.WriteLine("Username is already taken. Please choose another one.");
                    continue;
                }

                Console.WriteLine("Please enter a new password:");
                var newPassword = Console.ReadLine();
                if (newPassword != null) userToUpdate.Password = newPassword;

                await userRepository.UpdateAsync(userToUpdate);
                Console.WriteLine("User updated successfully.");
                updateIsComplete = true;
            }
        }


        private Task<bool> IsUsernameTakenAsync(string username)
        {
            var users = userRepository.GetMany();
            return Task.FromResult(users.Any(user => user.Name.Equals(username, StringComparison.OrdinalIgnoreCase)));
        }

        public Task ShowConsoleContent()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the user management console.");
            Console.WriteLine("Do you wish to (D)elete or (U)pdate");

            var input = Console.ReadLine();
            if (input is not { Length: 1 })
            {
                Console.WriteLine("Invalid input");
                return Task.CompletedTask;
            }

            switch (input.ToUpper())
            {
                case "D":
                    Console.WriteLine("Please enter the ID of the user you wish to delete:");
                    if (int.TryParse(Console.ReadLine(), out var deleteId))
                    {
                        DeleteUser(deleteId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID.");
                    }

                    break;
                case "U":
                    Console.WriteLine("Please enter the ID of the user you wish to update:");
                    if (int.TryParse(Console.ReadLine(), out var updateId))
                    {
                        UpdateUser(updateId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID.");
                    }

                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }

            return Task.CompletedTask;
        }
    }
}