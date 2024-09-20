using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private const string FilePath = "Data/users.json";

    public UserFileRepository()
    {
        if (!File.Exists(FilePath))
        {
            File.WriteAllText(FilePath, "[]");
        }
    }
    
    private static async Task<List<User>> SavedUsers()
    {
        var usersJson = await File.ReadAllTextAsync(FilePath);
        return JsonSerializer.Deserialize<List<User>>(usersJson)!;
    }
    
    private static async Task SaveUsers(List<User> users)
    {
        var usersJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(FilePath, usersJson);
    }
    
    public async Task<User> AddAsync(User user)
    {
        var usersSaved = await SavedUsers();
        
        var maxId = usersSaved.Count != 0 ? usersSaved.Max(x => x.Id) : 0;
        user.Id = maxId + 1;
        
        usersSaved.Add(user);

        await SaveUsers(usersSaved);

        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var usersSaved = await SavedUsers();
        
        var existingUser = usersSaved.SingleOrDefault(x => x.Id == user.Id);
        
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }
        
        usersSaved.Remove(existingUser);
        
        usersSaved.Add(user);
        
        await SaveUsers(usersSaved);
    }

    public async Task DeleteAsync(int id)
    {
        var usersSaved = await SavedUsers();
        
        usersSaved.Where(x => x.Id == id).ToList()
            .ForEach(x => usersSaved.Remove(x));
        
        await SaveUsers(usersSaved);
        
    }

    public async Task<User> GetSingleAsync(int id)
    {
        var usersSaved = await SavedUsers();
        
        var user = usersSaved.SingleOrDefault(x => x.Id == id);
        
        if (user is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }
        
        return user;
    }

    public IQueryable<User> GetMany()
    {
        var usersSaved = SavedUsers().Result;
        return usersSaved.AsQueryable();
    }
}