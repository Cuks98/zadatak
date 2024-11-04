using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.Settings;

namespace WebApplication1.Infrastructure;

internal sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Setting> Settings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setting>()
            .HasIndex(s => new { s.Id })
            .IsUnique();
    }
}