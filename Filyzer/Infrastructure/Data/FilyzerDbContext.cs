using Filyzer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Filyzer.Infrastructure.Data
{
    public class FilyzerDbContext(DbContextOptions<FilyzerDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Active).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.ApiKey).IsRequired();
                entity.HasIndex(e => e.ApiKey).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}
