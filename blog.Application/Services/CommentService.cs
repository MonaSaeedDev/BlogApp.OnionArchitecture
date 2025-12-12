using blog.Application.Abstractions.Services;
using blog.Application.DTOs.Comment;
using Blog.Domain.Entities;
using Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace blog.Application.Services;

public class CommentService(BlogDbContext context) : ICommentService
{ 
    public async Task<int> AddCommentAsync(CreateCommentDto newComment, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(newComment, nameof(newComment));

        if (string.IsNullOrWhiteSpace(newComment.Content))
        {
            throw new ArgumentException("Content cannot be null, empty, or whitespace.", nameof(newComment.Content));
        }

        var userExists = await context.Users.AnyAsync(u => u.Id == newComment.UserId, cancellationToken);
        if (!userExists) 
        {
            throw new ArgumentException("User not found", nameof(newComment.UserId));
        }

        var postExists = await context.Posts.AnyAsync(p => p.Id == newComment.PostId, cancellationToken);
        if (!postExists)
        {
            throw new ArgumentException("Post not found", nameof(newComment.PostId));
        }

        var comment = new Comment 
        { 
            UserId = newComment.UserId,
            PostId = newComment.PostId,
            Content = newComment.Content.Trim()
        };

        await context.Comments.AddAsync(comment, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return comment.Id;
    }

    public async Task<IReadOnlyList<CommentDto>> GetLatestCommentsAsync(LatestCommentsRequestDto request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(request.Take ?? 20);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(request.SinceHours ?? 24);

        var postExists = await context.Posts.AnyAsync(p => p.Id == request.PostId, cancellationToken);
        if (!postExists)
        {
            throw new ArgumentException("Post not found", nameof(request.PostId));
        }
        var sinceTime = DateTime.UtcNow.AddHours(-request.SinceHours!.Value);
        var comments =  context.Comments
            .AsNoTracking()
            .Where(c => c.PostId == request.PostId && c.CreatedAt >= sinceTime)
            .OrderByDescending(c => c.CreatedAt)
            .Take(request.Take!.Value)
            .Select(c => new CommentDto
            {
                Author = c.User.UserName,
                Content = c.Content,
                DisplayDate = c.UpdatedAt ?? c.CreatedAt
            });
            
        return await comments.ToListAsync(cancellationToken);
    }
}

