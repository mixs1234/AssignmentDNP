using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp(
    IUserRepository userRepository,
    ICommentRepository commentRepository,
    IPostRepository postRepository)
{
    private IUserRepository UserRepository { get; set; } = userRepository;
    private ICommentRepository CommentRepository { get; set; } = commentRepository;
    private IPostRepository PostRepository { get; set; } = postRepository;
    private IConsoleView? ConsoleView { get; set; }

    public async Task StartAsync()
    {
        
        var running = true;
        
        Console.Clear();
        
        while (running)
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("(M)anage + (P)osts, (C)omments, (U)sers");
            Console.WriteLine("(C)reate + (P)ost, (C)omment, (U)ser");
            Console.WriteLine("(V)iew + (P)ost, (C)omment, (U)ser");
            Console.WriteLine("(EE)xit");
            Console.WriteLine("--------------------------------------------");
            
            var input = Console.ReadLine();

            if (input.Length != 2)
            {
                Console.WriteLine("Invalid input");
                continue;
            }
            if (input.ToUpper().Equals("EE"))
            {
                running = false;
                continue;
            }

            switch (input.ToUpper()[..1])
            {
                case "M":
                    Manage(input);
                    break;
                case "C":
                    Create(input);
                    break;
                case "V":
                    View(input);
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    ConsoleView = null;
                    break;
            }
            
            if(ConsoleView != null)
                await ConsoleView.ShowConsoleContent();
            
        }
        
    }


    private void Manage(string input)
    {
        switch (input.ToUpper().Substring(1,1))
        {   
            case "P":
                ConsoleView = new ManagePostsView(PostRepository);
                break;
            case "C":
                ConsoleView = new ManageCommentsView(CommentRepository);
                break;
            case "U":
                ConsoleView = new ManageUsersView(UserRepository);
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
    }

    private void Create(string input)
    {
        switch (input.ToUpper().Substring(1,1))
        {
            case "P":
                ConsoleView = new CreatePostView(PostRepository);
                break;
            case "C":
                ConsoleView = new CreateCommentView(CommentRepository);
                break;
            case "U":
                ConsoleView = new CreateUserView(UserRepository);
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
    }

    private void View(string input)
    {
        switch (input.ToUpper().Substring(1, 1))
        {
            case "P":
                ConsoleView = new ListPostsView(PostRepository, CommentRepository);
                break;
            case "C":
                ConsoleView = new ListCommentsView(CommentRepository);
                break;
            case "U":
                ConsoleView = new ListUsersView(UserRepository);
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
    }



}