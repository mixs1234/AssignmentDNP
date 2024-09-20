using CLI.UI;
using FileRepositories;
using RepositoryContracts;

namespace CLI;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.Clear();
        
        Console.WriteLine("Starting CLI App...");
        
        IUserRepository userRepository = new UserFileRepository();
        ICommentRepository commentRepository = new CommentFileRepository();
        IPostRepository postRepository = new PostFileRepository();
        
        CliApp cliApp = new CliApp(userRepository, commentRepository, postRepository);
        await cliApp.StartAsync();
        
    }
}