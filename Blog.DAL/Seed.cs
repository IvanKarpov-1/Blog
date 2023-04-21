using Blog.DAL.DataContext;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog.DAL;

public class Seed
{
    public static async Task SeedData(BlogDataContext context, UserManager<User> manager)
    {
        var author = new User
        {
            Email = "author@gmail.com",
            DisplayName = "First Author",
            UserName = "first"
        };

        if (!manager.Users.Any())
        {
            var users = new List<User>
            {
                new()
                {
                    Email = "ffwawq@gmail.com",
                    DisplayName = "Ivan Karpov",
                    UserName = "ivan"
                },
                new()
                {
                    Email = "kokrocha@gmail.com",
                    DisplayName = "Jorjo Jovani",
                    UserName = "jorjo"
                },
                author
            };

            foreach (var user in users)
            {
                await manager.CreateAsync(user, "Pa$$w0rd");
            }
        }

        await context.SaveChangesAsync();

        var article = new Article
        {
            Author = author,
            Title = "First article",
            Description = "Some article description",
            Content =
                "The Dependency Inversion Principle (DIP) " +
                "is one of the SOLID design principles in object-oriented programming. " +
                "It states that high-level modules should not depend on low-level modules, " +
                "but both should depend on abstractions."
        };
        var comment = new Comment
        {
            Author = author,
            Parent = article,
            Content = "Some comment"
        };
        article.Comments = new List<Comment> { comment };

        if (!context.Articles.Any())
        {
            var users = context.Users.AsQueryable().ToList();

            var articles = new List<Article>
            {
                new ()
                {
                    Author = users[0],
                    Title = "Second Article",
                    Description = "Another article description",
                    Content = "In summary, adhering to the Interface Segregation " +
                              "Principle can lead to a more flexible, maintainable, " +
                              "and reusable codebase, while violating it can lead to " +
                              "bloated and fragile interfaces. Separation of concerns, " +
                              "decorator pattern, and adapter pattern are typical solutions " +
                              "for maintaining the principle."
                },
                new ()
                {
                    Author = users[2],
                    Title = "Second Article",
                    Description = "Simple description",
                    Content = "The Open-Closed Principle (OCP) is a fundamental principle of " +
                              "the SOLID design principles in object-oriented programming. " +
                              "It states that software entities (classes, modules, functions, etc.) " +
                              "should be open for extension but closed for modification."
                },
                article,
            };

            await context.Articles.AddRangeAsync(articles);
        }

        if (!context.Comments.Any())
        {
            var comments = new List<Comment>
            {
                new()
                {
                    Author = author,
                    Content = "Some third comment"
                }
            };

            await context.Comments.AddRangeAsync(comments);
        }

        var rubric = new Rubric
        {
            Articles = new List<Article> { article },
            Name = "Parent Rubric"
        };

        if (!context.Rubrics.Any())
        {
            var rubrics = new List<Rubric>
            {
                new()
                {
                    Name = "Children rubric 1",
                    Parent = rubric
                },
                new()
                {
                    Name = "Children rubric 2",
                    Parent = rubric
                }
            };

            rubric.Rubrics = rubrics;

            await context.Rubrics.AddAsync(rubric);
        }

        await context.SaveChangesAsync();
    }
}