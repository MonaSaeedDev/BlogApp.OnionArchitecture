using Blog.Domain.Enums;

namespace Blog.Domain.Entities;

public class Reaction : BaseEntity
{
    public int UserId { get; set; }
    public int? PostId { get; set; } 
    public int? CommentId { get; set; }
    public ReactionKind Kind { get; set; }
    public User User { get; set; } = null!;
    public Post? Post { get; set; } 
    public Comment? Comment { get; set; }
}
