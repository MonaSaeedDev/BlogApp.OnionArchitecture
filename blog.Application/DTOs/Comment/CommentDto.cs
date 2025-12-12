namespace blog.Application.DTOs.Comment;

public sealed record class CommentDto
{
    public required string Author { get; init; }
    public required string Content { get; init; }
    public required DateTime DisplayDate { get; init; }
}
