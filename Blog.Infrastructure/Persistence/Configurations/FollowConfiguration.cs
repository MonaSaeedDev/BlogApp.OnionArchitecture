using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Persistence.Configurations
{
    public class FollowConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder
                .ToTable("Follows");
            builder
                .Property(f => f.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();
            builder
                .ToTable (t => t.HasCheckConstraint("CK_Follow_SelfFollow", "[FollowerId] <> [FolloweeId]"));
            builder
                .HasKey(f => new { f.FollowerId, f.FolloweeId });
            builder
                .HasIndex(f => f.FollowerId);
            builder
               .HasIndex(f => f.FolloweeId);
            builder
                .HasOne(f => f.Follower)
                .WithMany(f => f.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
               .HasOne(f => f.Followee)
               .WithMany(f => f.Followers)
               .HasForeignKey(f => f.FolloweeId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
