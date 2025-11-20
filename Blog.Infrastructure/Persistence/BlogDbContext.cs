
using Microsoft.EntityFrameworkCore;
using Blog.Domain.Entities;
using Blog.Infrastructure.Persistence.Configurations;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Identity.Client;

namespace Blog.Infrastructure.Persistence
{
    public class BlogDbContext : DbContext
    {
        const string CONNECTION_STRING = "Server=.;Database=BlogDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
        public DbSet<User> Users => Set<User>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Reaction> Reactions => Set<Reaction>();
        public DbSet<Follow> Follows => Set<Follow>();

        public BlogDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(CONNECTION_STRING);
            }
        }
        public override int SaveChanges()
        {
            ApplyAuditColumns();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditColumns();
             return base.SaveChangesAsync(cancellationToken);
        }
        private void ApplyAuditColumns()
        {
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.Metadata.IsOwned())
                    continue;

                if (entry.State == EntityState.Added && entry.Property("CreatedAt") is { })
                {
                    entry.Property("CreatedAt").CurrentValue = now;
                }

                if (entry.State == EntityState.Modified && entry.Property("UpdatedAt") is { })
                {
                    entry.Property("UpdatedAt").CurrentValue = now;
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly); 


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.IsOwned())
                    continue;
                var entity = modelBuilder.Entity(entityType.ClrType);
                entity
                    .Property<bool>("IsDeleted")
                    .HasDefaultValue(false);
                entity
               .Property<byte[]>("RowVersion")
               .IsRowVersion()
               .IsConcurrencyToken();
            }
        }
    }
}
