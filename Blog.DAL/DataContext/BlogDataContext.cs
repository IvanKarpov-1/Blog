using Blog.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.DataContext;

public class BlogDataContext : IdentityDbContext<User>
{
    public BlogDataContext(DbContextOptions<BlogDataContext> options) : base(options) { }

    public new DbSet<T> Set<T>() where T : class
    {
        return base.Set<T>();
    }
    
    public DbSet<Rubric> Rubrics { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(c => c.Author)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Articles)
            .WithOne(c => c.Author)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Rubric>()
            .HasMany(r => r.Rubrics)
            .WithOne(r => r.Parent)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Rubric>()
            .HasMany(r => r.Articles)
            .WithOne(a => a.Rubric)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Comment>()
            .HasOne(p => p.Parent)
            .WithMany(c => c.Comments)
            .OnDelete(DeleteBehavior.Cascade);
    }
}