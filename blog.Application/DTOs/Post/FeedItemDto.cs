using System;
using System.Collections.Generic;
using System.Text;

namespace blog.Application.DTOs.Post
{
    public class FeedItemDto
    {
        public int PostId { get; set; }
        public string Author { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } 
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public bool YouLike { get; set; }
    }
}
