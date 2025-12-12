namespace blog.Application.DTOs.User;

public sealed record class CreateFollowDto
{
    public int FollowerId { get; init; }
    public int FolloweeId { get; init; }
}
