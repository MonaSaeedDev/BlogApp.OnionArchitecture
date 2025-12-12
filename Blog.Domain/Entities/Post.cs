namespace Blog.Domain.Entities;

public class Post : BaseEntity
{
    public int UserId { get; set; }
    public required string Content { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
}
