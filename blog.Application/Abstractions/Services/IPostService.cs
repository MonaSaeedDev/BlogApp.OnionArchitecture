using blog.Application.DTOs.Post;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace blog.Application.Abstractions.Services
{
    public interface IPostService
    {
        Task<int> CreatePostAsync(CreatePostDto createPostDto, CancellationToken cancellationToken = default);
        Task SoftDeletePostAsync(int postId, CancellationToken cancellationToken = default);
        Task RestorPostAsync(int postId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<FeedItemDto>> GetHomeFeedAsync(int userId, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    }
}
