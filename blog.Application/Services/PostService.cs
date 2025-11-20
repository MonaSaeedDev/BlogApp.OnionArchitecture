using blog.Application.Abstractions.Services;
using blog.Application.DTOs.Post;
using Blog.Domain.Entities;
using Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace blog.Application.Services
{
    public class PostService(BlogDbContext context) : IPostService
    {
        public async Task<int> CreatePostAsync(CreatePostDto createPostDto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createPostDto);
            ArgumentException.ThrowIfNullOrWhiteSpace(createPostDto.Content);

            var userExists = await context.Users
                .AnyAsync(u => u.Id == createPostDto.UserId, cancellationToken);

            if (!userExists)
            {
                throw new KeyNotFoundException(
                    $"Author with ID {createPostDto.UserId} was not found.");
            }
            var post = new Post
            {
                UserId = createPostDto.UserId,
                Content = createPostDto.Content,
                CreatedAt = DateTime.UtcNow
            };
            await context.Posts.AddAsync(post, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return post.Id; 
        }

        public async Task SoftDeletePostAsync(int postId, CancellationToken cancellationToken = default)
        {
            var post = await context.Posts.FindAsync(postId, cancellationToken);
            if (post is null || post.IsDeleted)
            {
                return;
            }

            post.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task RestorPostAsync(int postId, CancellationToken cancellationToken = default)
        {
            var post = await context.Posts.IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == postId && p.IsDeleted, cancellationToken);

            if (post is null)
            {
                return;
            }

            post.IsDeleted = false;
            await context.SaveChangesAsync(cancellationToken);
        }
        public async Task<IReadOnlyList<FeedItemDto>> GetHomeFeedAsync(int userId, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        { 
            if (page <= 0)
            { 
                throw new ArgumentOutOfRangeException(nameof(page), "Page number must be greater than zero.");
            }
            if (pageSize <= 0 || pageSize > 100)
            { 
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 100.");
            }

            var isExistUser = await context.Users.AnyAsync(u => u.Id == userId, cancellationToken);
            if (!isExistUser) return new List<FeedItemDto>(); 

                var followees = context.Follows 
                    .Where(f => f.FollowerId == userId)
                    .Select(f => f.FolloweeId); 
                
                // Project feed items directly in SQL to avoid loading entity graphs. this comment
                var feedQuery = context.Posts 
                    .AsNoTracking()
                    .Where(p => followees.Contains(p.UserId))
                    .OrderBy(p=> p.CreatedAt)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Select(p => new FeedItemDto
                    { 
                        PostId = p.Id,
                        Author = p.User != null ? p.User.UserName : "Unknown",
                        Content = p.Content ?? "",
                        CreatedAt = p.CreatedAt,
                        LikeCount = p.Reactions.Count(),
                        CommentCount = p.Comments.Count(),
                        YouLike = p.Reactions.Any(r => r.UserId == userId),
                    });

                return await feedQuery.ToListAsync(cancellationToken); 
        }
    }
}
