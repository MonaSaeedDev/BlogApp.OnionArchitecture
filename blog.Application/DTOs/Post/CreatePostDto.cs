namespace blog.Application.DTOs.Post;

public sealed record class CreatePostDto
{
    public required int UserId { get; init; }
    public required string Content { get; init; }
}
