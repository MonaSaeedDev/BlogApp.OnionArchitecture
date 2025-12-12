namespace blog.Application.DTOs.Post;

public sealed record class FeedItemDto
{
    public required int PostId { get; init; }
    public required string Author { get; init; }
    public required string Content { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required int LikeCount { get; init; }
    public required int CommentCount { get; init; }
    public required bool YouLike { get; init; }
}
