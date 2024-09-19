using RepositoryContracts;

namespace CLI.UI.ManageComments
{
    public class ManageCommentsView(ICommentRepository commentRepository) : IConsoleView
    {
        private async Task DeleteComment(int id)
        {
            await commentRepository.DeleteAsync(id);
        }

        private async Task UpdateComment(int id)
        {
            var commentToUpdate = await commentRepository.GetSingleAsync(id);
            var updateIsComplete = false;

            while (!updateIsComplete)
            {
                Console.WriteLine("Please enter the new content of the comment:");
                var newContent = Console.ReadLine();

                if (!string.IsNullOrEmpty(newContent))
                {
                    commentToUpdate.Body = newContent;
                    await commentRepository.UpdateAsync(commentToUpdate);
                    Console.WriteLine("Comment updated successfully.");
                    updateIsComplete = true;
                }
                else
                {
                    Console.WriteLine("Content cannot be empty. Please enter valid content.");
                }
            }
        }

        public Task ShowConsoleContent()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the comment management console.");
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
                    Console.WriteLine("Please enter the ID of the comment you wish to delete:");
                    if (int.TryParse(Console.ReadLine(), out var deleteId))
                    {
                        DeleteComment(deleteId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID.");
                    }

                    break;
                case "U":
                    Console.WriteLine("Please enter the ID of the comment you wish to update:");
                    if (int.TryParse(Console.ReadLine(), out var updateId))
                    {
                        UpdateComment(updateId);
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
