using Entities;

namespace RepositoryContracts;

public interface IPostRepository
{
    //takes a Post, and returns the created Post.
    Task<Post> AddAsync(Post post);
    
    //takes a Post (with ID), and just replaces the existing Post. If no existing Post is found, an exception is thrown
    //to indicate the error.
    Task UpdateAsync(Post post);
    
    //takes an ID, and deletes the Post with that ID. If no Post is found, an exception is thrown to indicate the error.
    Task DeleteAsync(int id);
    
    //takes an ID, and returns the Post with that ID. If no Post is found, an exception is thrown to indicate the error.
    Task<Post> GetSingleAsync(int id);
    
    //returns all Posts.
    IQueryable<Post> GetMany();
}