using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApp.Controller;

[ApiController]
[Route("api/Posts/{postId:int}/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    
    public CommentsController(ICommentRepository commentRepository, IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetMany([FromQuery] int? userId = null, [FromQuery] string? userName = null, [FromQuery] int? postId = null)
    {
        var comments = _commentRepository.GetMany();

        if (userId.HasValue)
        {
            comments = comments.Where(comment => comment.UserId == userId.Value);
        }

        if (!string.IsNullOrEmpty(userName))
        {
            var users = _userRepository.GetMany().Where(user => user.Name.Contains(userName, StringComparison.OrdinalIgnoreCase)).Select(user => user.Id);
            comments = comments.Where(comment => users.Contains(comment.UserId));
        }

        if (postId.HasValue)
        {
            comments = comments.Where(comment => comment.PostId == postId.Value);
        }

        var commentDtos = comments.Select(comment => new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            PostId = comment.PostId,
            UserId = comment.UserId
        }).ToList();

        return Ok(commentDtos);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingle(int id)
    {
        var comment = await _commentRepository.GetSingleAsync(id);
        var commentDto = new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            PostId = comment.PostId,
            UserId = comment.UserId
        };
        
        return Ok(commentDto);
    }
    
    [HttpPost]
    public async Task<ActionResult<CommentDto>> Add(CommentDto commentDto)
    {
        var comment = new Comment
        {
            Body = commentDto.Body,
            PostId = commentDto.PostId,
            UserId = commentDto.UserId
        };
        
        var createdComment = await _commentRepository.AddAsync(comment);
        var createdCommentDto = new CommentDto
        {
            Id = createdComment.Id,
            Body = createdComment.Body,
            PostId = createdComment.PostId,
            UserId = createdComment.UserId
        };
        
        return Ok(createdCommentDto);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CommentDto>> Update(int id, CommentDto commentDto)
    {
        var comment = new Comment
        {
            Id = id,
            Body = commentDto.Body,
            PostId = commentDto.PostId,
            UserId = commentDto.UserId
        };
        
        await _commentRepository.UpdateAsync(comment);
        
        return Ok(commentDto);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _commentRepository.DeleteAsync(id);
        
        return NoContent();
    }
    
}