using System.Collections.ObjectModel;
using ApiContracts;

namespace BlazorApp.Services.Comments;

public interface ICommentService
{
    public Task<CommentDTO> AddCommentAsync(CreateCommentDTO request, int postId);
}