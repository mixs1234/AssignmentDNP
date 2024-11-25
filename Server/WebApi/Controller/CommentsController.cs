using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApp.Controller;

[ApiController]
[Route("api/Posts/{postId:int}/[controller]")]
public class CommentsController(ICommentRepository commentRepository, IUserRepository userRepository)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDTO>>> GetMany(int? postId,
        [FromQuery] int? userId = null,
        [FromQuery] string? userName = null)
    {
        var comments = commentRepository.GetMany();

        if (userId.HasValue)
        {
            comments = comments.Where(comment => comment.UserId == userId.Value);
        }

        if (!string.IsNullOrEmpty(userName))
        {
            var users = userRepository.GetMany().Where(user => user.Name.Contains(userName, StringComparison.OrdinalIgnoreCase)).Select(user => user.Id);
            comments = comments.Where(comment => users.Contains(comment.UserId));
        }

        if (postId.HasValue)
        {
            comments = comments.Where(comment => comment.PostId == postId.Value);
        }

        var commentDtos = comments.Select(comment => new CommentDTO
        {
            Id = comment.Id,
            Body = comment.Body,
            PostId = comment.PostId,
            UserId = comment.UserId
        }).ToList();

        return Ok(commentDtos);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDTO>> GetSingle(int id)
    {
        var comment = await commentRepository.GetSingleAsync(id);
        var commentDto = new CommentDTO
        {
            Id = comment.Id,
            Body = comment.Body,
            PostId = comment.PostId,
            UserId = comment.UserId
        };
        
        return Ok(commentDto);
    }
    
    [HttpPost]
    public async Task<ActionResult<CommentDTO>> Add(CreateCommentDTO createCommentDto, int postId)
    {
        var comment = new Comment(createCommentDto.Body, postId, createCommentDto.UserId);
        
        var createdComment = await commentRepository.AddAsync(comment);
        var createdCommentDto = new CommentDTO
        {
            Id = createdComment.Id,
            Body = createdComment.Body,
            PostId = createdComment.PostId,
            UserId = createdComment.UserId
        };
        
        return Ok(createdCommentDto);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CommentDTO>> Update(int id, UpdateCommentDTO updateCommentDto)
    {
        var comment = new Comment(updateCommentDto.Body, updateCommentDto.PostId, updateCommentDto.UserId)
        {
            Id = id
        };
        
        await commentRepository.UpdateAsync(comment);
        
        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await commentRepository.DeleteAsync(id);
        
        return NoContent();
    }
    
}