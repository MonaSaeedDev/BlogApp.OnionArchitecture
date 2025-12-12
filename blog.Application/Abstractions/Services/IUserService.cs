using blog.Application.DTOs.User;
namespace blog.Application.Abstractions.Services;

public interface IUserService
{
    Task<int> CreateUserAsync(CreateUserDto newUser, CancellationToken cancellationToken = default);
    Task FollowAsync(CreateFollowDto newFollow, CancellationToken cancellationToken = default);
    Task UnfollowAsync(UnfollowDto newFollow, CancellationToken cancellationToken = default);
}
