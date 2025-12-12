namespace blog.Application.DTOs.User;

public sealed record class UnfollowDto
{
    public int FollowerId { get; init; }
    public int FolloweeId { get; init; }

}
