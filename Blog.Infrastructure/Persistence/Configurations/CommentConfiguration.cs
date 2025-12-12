using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.Configurations;  

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(c => c.Id);

        builder
            .Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(500);
        builder.HasQueryFilter(c => !c.IsDeleted);
        builder
            .HasIndex(c => new { c.PostId, c.CreatedAt });
        builder
            .HasOne(u => u.User)
            .WithMany(c => c.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
          .HasOne(u => u.Post)
          .WithMany(c => c.Comments)
          .HasForeignKey(c => c.PostId)
          .OnDelete(DeleteBehavior.Cascade);
    }
}
