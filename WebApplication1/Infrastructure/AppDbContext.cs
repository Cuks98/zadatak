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
        
        modelBuilder.Entity<Setting>().HasData(
            new Setting { Id = 1, Value = "12.0%", ValidFrom = new DateTime(2020, 1, 1) },
            new Setting { Id = 2, Value = "10.0%", ValidFrom = new DateTime(2022, 1, 1)  }
        );
    }
}