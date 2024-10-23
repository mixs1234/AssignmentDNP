using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;

    public PostsController(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetMany([FromQuery] string? titleContains = null, [FromQuery] int? userId = null, [FromQuery] string? userName = null)
    {
        var posts = _postRepository.GetMany();

        if (!string.IsNullOrEmpty(titleContains))
        {
            posts = posts.Where(post => post.Title.Contains(titleContains, StringComparison.OrdinalIgnoreCase));
        }

        if (userId.HasValue)
        {
            posts = posts.Where(post => post.UserId == userId.Value);
        }

        if (!string.IsNullOrEmpty(userName))
        {
            var users = _userRepository.GetMany().Where(user => user.Name.Contains(userName, StringComparison.OrdinalIgnoreCase)).Select(user => user.Id);
            posts = posts.Where(post => users.Contains(post.UserId));
        }

        var postDtos = posts.Select(post => new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId,
            Comments = _commentRepository.GetMany()
                .Where(comment => comment.PostId == post.Id)
                .Select(comment => new CommentDto
                {
                    Id = comment.Id,
                    Body = comment.Body,
                    PostId = comment.PostId,
                    UserId = comment.UserId
                }).ToList()
        }).ToList();

        return Ok(postDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSingle(int id, bool includeComments = false)
    {
        var post = await _postRepository.GetSingleAsync(id);
        PostDto postDto;
        if (!includeComments)
        {
            postDto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                Comments = (List<CommentDto>) []
            };
        }
        else
        {
            postDto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                Comments = _commentRepository.GetMany()
                    .Where(comment => comment.PostId == post.Id)
                    .Select(comment => new CommentDto
                    {
                        Id = comment.Id,
                        Body = comment.Body,
                        PostId = comment.PostId,
                        UserId = comment.UserId
                    }).ToList()
            };
        }

        return Ok(postDto);
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> Create(PostDto postDto)
    {
        var post = new Post
        {
            Title = postDto.Title,
            Body = postDto.Body,
            UserId = postDto.UserId
        };

        var createdPost = await _postRepository.AddAsync(post);
        postDto.Id = createdPost.Id;

        return CreatedAtAction(nameof(GetSingle), new { id = postDto.Id }, postDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, PostDto postDto)
    {
        var post = new Post
        {
            Id = id,
            Title = postDto.Title,
            Body = postDto.Body,
            UserId = postDto.UserId
        };

        await _postRepository.UpdateAsync(post);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _postRepository.DeleteAsync(id);
        return NoContent();
    }
}