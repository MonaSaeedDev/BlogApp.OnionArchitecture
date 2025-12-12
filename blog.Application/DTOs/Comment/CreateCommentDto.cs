namespace blog.Application.DTOs.Comment;

public sealed record class CreateCommentDto
{
    public required int UserId { get; init; }
    public required int PostId { get; init; }
    public required string Content { get; init; }
}
