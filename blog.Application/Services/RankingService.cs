using blog.Application.Abstractions.Services;
using blog.Application.DTOs.Post;
using Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace blog.Application.Services;

public class RankingService(BlogDbContext context) : IRankingService 
{
    public async Task<IReadOnlyList<RankingPostDto>> TopPostsAsync(int days = 7, int take = 10, CancellationToken cancellationToken = default)
    {
        if(days <= 0)
        {
            throw new ArgumentException(nameof(days));
        }
        if (take <= 0)
        {
            throw new ArgumentException(nameof(take));
        }
        var sinceTime = DateTime.UtcNow.AddDays(-days);

        var topPostsQuery =
             context.Posts
            .AsNoTracking()
            .Where(p => p.CreatedAt >= sinceTime)
            .Select(p => new RankingPostDto
            {
                PostId = p.Id,
                Snippet = p.Content.Length >= 80 ? p.Content.Substring(0, 80) + " ..." : p.Content,
                Score = p.Reactions.Count() * 2d + p.Comments.Count()
            })
            .OrderByDescending(p => p.Score)
            .ThenByDescending(p => p.PostId)
            .Take(take);

        return await topPostsQuery.ToListAsync(cancellationToken);
    }
}
