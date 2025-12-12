using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => new { p.UserId, p.CreatedAt })
            .IsDescending(false, true);
        builder
            .Property(p => p.Content)
            .IsRequired()
            .HasMaxLength(1000); 
        builder.HasQueryFilter(p => !p.IsDeleted);
        builder
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
