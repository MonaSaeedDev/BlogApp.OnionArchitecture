using System;
using System.Collections.Generic;
using System.Text;

namespace blog.Application.DTOs.Post
{
    public class CreatePostDto
    {
        public int UserId { get; set; }
        public string Content { get; set; } = null!;
    }
}
