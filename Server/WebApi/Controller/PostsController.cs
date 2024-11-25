using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class PostsController(
    IPostRepository postRepository)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDTO>>> GetMany(
        [FromQuery] string? titleContains = null,
        [FromQuery] int? userId = null,
        [FromQuery] string? userName = null,
        [FromQuery] bool includeComments = false)
    {
        var queryForPosts = postRepository.GetMany()
            .AsQueryable();

        if (!string.IsNullOrEmpty(titleContains))
        {
            queryForPosts = queryForPosts.Where(post => post.Title.Contains(titleContains, StringComparison.OrdinalIgnoreCase));
        }

        if (userId.HasValue)
        {
            queryForPosts = queryForPosts.Where(post => post.UserId == userId.Value);
        }

        if (!string.IsNullOrEmpty(userName))
        {
            queryForPosts = queryForPosts.Where(post => post.User.Name.Contains(userName, StringComparison.OrdinalIgnoreCase));
        }

        if (includeComments)
        {
            queryForPosts = queryForPosts.Include(post => post.Comments);
        }

        var postDtos = await queryForPosts.Select(post => new PostDTO()
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                Comments = includeComments
                    ? new List<CommentDTO>(post.Comments.Select(comment => new CommentDTO()
                    {
                        Id = comment.Id,
                        Body = comment.Body,
                        PostId = comment.PostId,
                        UserId = comment.UserId
                    })).ToList()
                    : new()
            })
            .ToListAsync();

        return Ok(postDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDTO>> GetSingle(
        [FromRoute] int id,
        [FromQuery] bool includeComments = false,
        [FromQuery] bool includeUser = false)
    {
        var queryForPosts = postRepository.GetMany()
            .Where(post => post.Id == id)
            .AsQueryable();

        if (includeUser)
        {
            queryForPosts = queryForPosts.Include(post => post.User);
        }

        if (includeComments)
        {
            queryForPosts = queryForPosts.Include(post => post.Comments);
        }

        PostDTO? postDto = await queryForPosts.Select(post => new PostDTO()
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                User = includeUser
                    ? new UserDTO()
                    {
                        Id = post.User!.Id,
                        Username = post.User.Name,
                        Password = post.User.Password
                    }
                    : null,
                Comments = includeComments
                    ? new List<CommentDTO>(post.Comments.Select(comment => new CommentDTO()
                    {
                        Id = comment.Id,
                        Body = comment.Body,
                        PostId = comment.PostId,
                        UserId = comment.UserId
                    })).ToList()
                    : new()
            })
            .FirstOrDefaultAsync();
        
        return postDto == null ? NotFound() : Ok(postDto);
    }

    [HttpPost]
    public async Task<ActionResult<PostDTO>> Create(CreatePostDTO createPostDto)
    {
        var post = new Post(createPostDto.Title, createPostDto.Body, createPostDto.UserId);
        
        var createdPost = await postRepository.AddAsync(post);

        return Ok(createdPost);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id,
        UpdatePostDTO updatePostDto)
    {
        var post = new Post(updatePostDto.Title, updatePostDto.Body, updatePostDto.UserId)
        {
            Id = id
        };
        
        await postRepository.UpdateAsync(post);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await postRepository.DeleteAsync(id);
        return NoContent();
    }
}