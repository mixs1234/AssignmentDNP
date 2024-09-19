using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

namespace CLI;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Clear();
        
        Console.WriteLine("Starting CLI App...");
        
        IUserRepository userRepository = new UserInMemoryRepository();
        ICommentRepository commentRepository = new CommentInMemoryRepository();
        IPostRepository postRepository = new PostInMemoryRepository();
        
        CliApp cliApp = new CliApp(userRepository, commentRepository, postRepository);
        await cliApp.StartAsync();
        
    }
}