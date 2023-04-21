namespace Blog.DAL.Models;

public class Article : Commentable
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    
    public User Author { get; set; }
    public Rubric Rubric { get; set; }
}