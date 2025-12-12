namespace Blog.Domain.Entities;

public class Comment : BaseEntity
{
    public int? PostId { get; set; }
    public int? UserId { get; set; }
    public required string Content { get; set; }
    public User User { get; set; } = null!;
    public Post Post { get; set; } = null!;
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
}
