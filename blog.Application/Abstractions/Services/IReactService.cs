using blog.Application.DTOs.React;

namespace blog.Application.Abstractions.Services;

public interface IReactService
{
    Task ReactToPostAsync(CreateReactDto newReact, CancellationToken cancellationToken);
}
