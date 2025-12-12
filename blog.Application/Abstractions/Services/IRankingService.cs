using blog.Application.DTOs.Post;

namespace blog.Application.Abstractions.Services;

public interface IRankingService
{
    Task<IReadOnlyList<RankingPostDto>> TopPostsAsync(int days, int take, CancellationToken cancellationToken);
}
