using RepositoryContracts;

namespace CLI.UI.ManagePosts
{
    public class ManagePostsView(IPostRepository postRepository) : IConsoleView
    {
        private async Task DeletePost(int id)
        {
            await postRepository.DeleteAsync(id);
        }

        private async Task UpdatePost(int id)
        {
            var postToUpdate = await postRepository.GetSingleAsync(id);
            var updateIsComplete = false;

            while (!updateIsComplete)
            {
                Console.WriteLine("Please enter the new title of the post:");
                var newTitle = Console.ReadLine();

                if (!string.IsNullOrEmpty(newTitle))
                {
                    postToUpdate.Title = newTitle;
                }
                else
                {
                    Console.WriteLine("Title cannot be empty. Please enter a valid title.");
                    continue;
                }

                Console.WriteLine("Please enter the new content of the post:");
                var newContent = Console.ReadLine();

                if (!string.IsNullOrEmpty(newContent))
                {
                    postToUpdate.Body = newContent;
                }
                else
                {
                    Console.WriteLine("Content cannot be empty. Please enter valid content.");
                    continue;
                }

                await postRepository.UpdateAsync(postToUpdate);
                Console.WriteLine("Post updated successfully.");
                updateIsComplete = true;
            }
        }

        public Task ShowConsoleContent()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the post management console.");
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
                    Console.WriteLine("Please enter the ID of the post you wish to delete:");
                    if (int.TryParse(Console.ReadLine(), out var deleteId))
                    {
                        DeletePost(deleteId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID.");
                    }

                    break;
                case "U":
                    Console.WriteLine("Please enter the ID of the post you wish to update:");
                    if (int.TryParse(Console.ReadLine(), out var updateId))
                    {
                        UpdatePost(updateId);
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
