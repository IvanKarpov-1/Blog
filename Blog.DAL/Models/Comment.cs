using Blog.DAL.Contracts;

namespace Blog.DAL.Models;

public class Comment : Commentable, IHierarchical<Commentable>
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string Content { get; set; }
    
    public User Author { get; set; }
    public Commentable Parent { get; set; }
}