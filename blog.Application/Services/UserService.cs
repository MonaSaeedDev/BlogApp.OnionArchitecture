using blog.Application.Abstractions.Services;
using blog.Application.DTOs.User;
using Blog.Domain.Entities;
using Blog.Domain.ValueObjects;
using Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace blog.Application.Services;

public class UserService(BlogDbContext context) : IUserService 
{
    public async Task<int> CreateUserAsync(CreateUserDto newUser, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(newUser, nameof(newUser));

        if (string.IsNullOrWhiteSpace(newUser.UserName))
        {
            throw new ArgumentException("User name cannot be null, empty, or whitespace.", nameof(newUser.UserName));
        }

        var normalizedUserName = newUser.UserName.Trim().ToLowerInvariant();
        if (await context.Users.AnyAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken))
        {
            throw new ArgumentException($"Username {normalizedUserName} is already taken.");
        }

        if (string.IsNullOrWhiteSpace(newUser.Email))
        {
            throw new ArgumentException("Email cannot be null, empty, or whitespace.", nameof(newUser.Email));
        }

        var user = new User
        {
            UserName = newUser.UserName,          
            NormalizedUserName = normalizedUserName,
            Email = Email.Create(newUser.Email),
            Bio = newUser.Bio?.Trim(),
        };

        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
    public async Task FollowAsync(CreateFollowDto newFollow, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(newFollow);

        if (newFollow.FollowerId == newFollow.FolloweeId)
        {
            throw new InvalidOperationException("Users cannot follow themselves.");
        }

        bool exists = await context.Follows.AnyAsync(
            f => f.FollowerId == newFollow.FollowerId &&
                 f.FolloweeId == newFollow.FolloweeId,
            cancellationToken
        );
        if (exists)
        {
            throw new InvalidOperationException("This follow relationship already exists.");
        }

        var followerExists = await context.Users.AnyAsync(u => u.Id == newFollow.FollowerId, cancellationToken);
        if (!followerExists)
        {
            throw new ArgumentException("Follower does not exist.");
        }

        var followeeExists = await context.Users.AnyAsync(u => u.Id == newFollow.FolloweeId, cancellationToken);
        if (!followeeExists)
        {
            throw new ArgumentException("Followee does not exist.");
        }

        var follow = new Follow()
        {
            FollowerId = newFollow.FollowerId,
            FolloweeId = newFollow.FolloweeId
        };

        await context.Follows.AddAsync(follow, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task UnfollowAsync(UnfollowDto newFollow, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(newFollow, nameof(newFollow));

        if (newFollow.FollowerId == newFollow.FolloweeId)
            throw new InvalidOperationException("Users cannot unfollow themselves.");

        var follow = await context.Follows.FirstOrDefaultAsync(
         f => f.FollowerId == newFollow.FollowerId &&
              f.FolloweeId == newFollow.FolloweeId,
         cancellationToken
     );
        if (follow is null)
        {
            throw new InvalidOperationException("Follow relationship does not exist.");
        }

        context.Follows.Remove(follow!);
        await context.SaveChangesAsync(cancellationToken);
    }
}
