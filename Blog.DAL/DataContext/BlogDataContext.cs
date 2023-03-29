using Microsoft.EntityFrameworkCore;
using Blog.DAL.Models;

namespace Blog.DAL.DataContext;

public class BlogDataContext : DbContext
{
    public BlogDataContext(DbContextOptions<BlogDataContext> options) : base(options) { }

    public new DbSet<T> Set<T>() where T : class
    {
        return base.Set<T>();
    }

    public DbSet<User> Users { get; set; }
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
            .HasOne(r => r.Parent)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Rubric>()
            .HasMany(r => r.Articles)
            .WithOne(a => a.Rubric)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void ExecuteCommand(string command, params object[] parameters)
    {
        base.Database.ExecuteSqlRaw(command, parameters);
    }
}