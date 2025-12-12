using blog.Application.Abstractions.Services;
using blog.Application.DTOs.React;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace blog.Application.Services;

public class ReactService(BlogDbContext context) : IReactService
{
    public async Task ReactToPostAsync(CreateReactDto newReact, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(newReact, nameof(newReact));
        
        if (!await context.Users.AnyAsync(u => u.Id == newReact.UserId, cancellationToken))
        {
            throw new ArgumentException("User not found.", nameof(newReact.UserId));
        }

        if (!await context.Posts.AnyAsync(u => u.Id == newReact.PostId, cancellationToken))
        {
            throw new ArgumentException("Post not found.", nameof(newReact.PostId));
        }

        if (!Enum.IsDefined(typeof(ReactionKind), newReact.Kind))
        {
            throw new ArgumentException("Invalid reaction.", nameof(newReact.Kind));
        }

        var react = await context.Reactions
            .FirstOrDefaultAsync(r => r.UserId == newReact.UserId && r.PostId == newReact.PostId, cancellationToken);

        if (react is null)
        {
            await context.AddAsync(new Reaction
            {
                UserId = newReact.UserId,
                PostId = newReact.PostId,
                Kind = newReact.Kind
            }, cancellationToken);
        }
        else if (react.Kind != newReact.Kind)
        {
            react.Kind = newReact.Kind;
        }

            await context.SaveChangesAsync(cancellationToken);
    }
}