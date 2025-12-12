using Blog.Domain.Enums;

namespace blog.Application.DTOs.React;

public sealed record class CreateReactDto
{
    public required int UserId { get; init; }
    public required int PostId { get; init; }
    public required ReactionKind Kind { get; init; }
}

