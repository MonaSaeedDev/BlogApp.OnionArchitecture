using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.Configurations;

public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
{
    public void Configure(EntityTypeBuilder<Reaction> builder)
    {
        builder.ToTable("Reactions");

        builder.HasKey(r => r.Id);

        builder
           .ToTable(t => t.HasCheckConstraint("CK_Reaction_HasTarget", "[PostId] IS NOT NULL OR [CommentId] IS NOT NULL"));

        builder.Property(r => r.Kind)
            .HasConversion<string>()
            .HasMaxLength(20);
        
        builder.HasIndex(r => new { r.PostId, r.UserId, r.Kind }).IsUnique();

        builder.HasIndex(r => new { r.CommentId, r.UserId, r.Kind }).IsUnique();

        builder.HasIndex(r => r.Id).IsUnique();

        builder.HasQueryFilter(r => !r.IsDeleted);
        
        builder.HasOne(r => r.Post)
            .WithMany(p => p.Reactions)
            .HasForeignKey(r => r.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(r => r.User)
           .WithMany(u => u.Reactions)
           .HasForeignKey(r => r.UserId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Comment)
           .WithMany(u => u.Reactions)
           .HasForeignKey(r => r.CommentId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}
