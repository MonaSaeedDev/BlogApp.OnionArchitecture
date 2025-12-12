namespace blog.Application.DTOs.User;

public sealed record class CreateUserDto
{
    public required string UserName { get; init; }
    public required string Email { get; init; }
    public string? Bio { get; init; } 
}
