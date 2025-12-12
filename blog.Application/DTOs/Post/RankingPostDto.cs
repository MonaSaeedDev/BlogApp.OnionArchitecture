namespace blog.Application.DTOs.Post;

public sealed record class RankingPostDto
{
    public required int PostId { get; init; }
    public required double Score { get; init; }
    public required string Snippet { get; init; } = null!;
}
