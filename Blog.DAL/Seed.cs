using Blog.DAL.DataContext;
using Blog.DAL.Models;

namespace Blog.DAL;

public class Seed
{
    public static async Task SeedData(BlogDataContext context)
    {
        var user = new User
        {
            Email = "author@gmailcom",
            Name = "First Author",
            Password = "qweqwe"
        };

        if (!context.Users.Any())
        {
            var users = new List<User>()
            {
                new()
                {
                    Email = "ffwawq@gmail.com",
                    Name = "Ivan Karpov",
                    Password = "11322ffaa"
                },
                new()
                {
                    Email = "kokrocha@gmail.com",
                    Name = "Jorjo Jovani",
                    Password = "ThaWarudo"
                },
                user
            };

            await context.Users.AddRangeAsync(users);
        }

        if (!context.Rubrics.Any())
        {
            var rubrics = new List<Rubric>()
            {
                new()
                {
                    Description = "Some description",
                    Name = "Some rubric"
                }
            };

            await context.Rubrics.AddRangeAsync(rubrics);
        }


        var article = new Article
        {
            Author = user,
            Title = "First article"
        };
        var comment = new Comment
        {
            Author = user,
            Parent = article,
            Content = "Some comment"
        };
        article.Comments = new List<Comment> { comment };

        if (!context.Articles.Any())
        {
            await context.Articles.AddRangeAsync(article);
        }

        if (!context.Comments.Any())
        {
            await context.Comments.AddRangeAsync(comment);
        }


        await context.SaveChangesAsync();
    }
}