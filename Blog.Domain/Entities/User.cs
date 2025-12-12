using Blog.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Domain.Entities;

public class User : BaseEntity
{
    public required string UserName { get; set; }
    public required string NormalizedUserName { get; set; }
    public required Email Email { get; set; }
    public string? Bio { get; set; }
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

    [InverseProperty(nameof(Follow.Followee))]
    public ICollection<Follow> Followers { get; set; } = new List<Follow>();

    [InverseProperty(nameof(Follow.Follower))]
    public ICollection<Follow> Following { get; set; } = new List<Follow>();
}
