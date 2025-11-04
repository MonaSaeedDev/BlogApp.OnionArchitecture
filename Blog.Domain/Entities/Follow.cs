using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blog.Domain.Entities
{ 
    public class Follow // many-to-many self-relationship on the User table
    {  
        public required int FollowerId { get; set; }   
        public required int FolloweeId { get; set; }   

        [InverseProperty(nameof(User.Following))]
        public User Follower { get; set; } = null!; 

        [InverseProperty(nameof(User.Followers))]
        public User Followee { get; set; } = null!;  
        public DateTime CreatedAt { get; set; }
    }
}
