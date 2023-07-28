using Microsoft.EntityFrameworkCore;
using P330Pronia.Models;

namespace P330Pronia.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Slider> Sliders { get; set; } = null!;
    public DbSet<Feature> Features { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feature>().HasQueryFilter(f => !f.IsDeleted);
        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
        base.OnModelCreating(modelBuilder);
    }
}