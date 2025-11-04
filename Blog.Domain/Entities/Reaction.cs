using System;
using System.Collections.Generic;
using Blog.Domain.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
    public class Reaction : BaseEntity
    {
        public int UserId { get; set; }// reaction without post and comment
        public int? PostId { get; set; } 
        public int? CommentId { get; set; }
        public ReactionKind Kind { get; set; }
        public User User { get; set; } = null!;
        public Post? Post { get; set; } 
        public Comment? Comment { get; set; }
    }
}
