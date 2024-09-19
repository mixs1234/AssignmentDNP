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

    private async Task Initialize()
    {
        UserRepository.AddAsync(new User{Name = "John", Password = "1234"});
        UserRepository.AddAsync(new User{Name = "Jane", Password = "1934"});
        UserRepository.AddAsync(new User{Name = "Jack", Password = "8463"});
        UserRepository.AddAsync(new User{Name = "Jill", Password = "9232"});
        UserRepository.AddAsync(new User{Name = "James", Password = "3823"});
        
        PostRepository.AddAsync(new Post{Title = "Post 1", Body = "Body 1", UserId = 1});
        PostRepository.AddAsync(new Post{Title = "Post 2", Body = "Body 2", UserId = 2});
        PostRepository.AddAsync(new Post{Title = "Post 3", Body = "Body 3", UserId = 3});
        PostRepository.AddAsync(new Post{Title = "Post 4", Body = "Body 4", UserId = 4});
        PostRepository.AddAsync(new Post{Title = "Post 5", Body = "Body 5", UserId = 5});
        
        CommentRepository.AddAsync(new Comment{Body = "Comment 1", PostId = 1, UserId = 1});
        CommentRepository.AddAsync(new Comment{Body = "Comment 2", PostId = 2, UserId = 2});
        CommentRepository.AddAsync(new Comment{Body = "Comment 3", PostId = 3, UserId = 3});
        CommentRepository.AddAsync(new Comment{Body = "Comment 4", PostId = 4, UserId = 4});
        CommentRepository.AddAsync(new Comment{Body = "Comment 5", PostId = 5, UserId = 5});
    }
    
    public async Task StartAsync()
    {
        
        await Initialize();
        
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
                ConsoleView = new ListPostsView(PostRepository);
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