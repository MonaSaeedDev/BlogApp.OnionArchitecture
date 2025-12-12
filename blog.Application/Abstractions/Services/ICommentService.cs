using blog.Application.DTOs.Comment;

namespace blog.Application.Abstractions.Services;

public interface ICommentService
{
    Task<int> AddCommentAsync(CreateCommentDto newComment, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CommentDto>> GetLatestCommentsAsync(LatestCommentsRequestDto request, CancellationToken cancellationToken = default);
}
