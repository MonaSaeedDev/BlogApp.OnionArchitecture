namespace blog.Application.DTOs.Comment;

public sealed record class LatestCommentsRequestDto
{
    public required int PostId { get; init; }
    public int? Take { get; init; }     
    public int? SinceHours { get; init; } 
}
