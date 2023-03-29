using System.ComponentModel.DataAnnotations.Schema;
using Blog.DAL.Contracts;

namespace Blog.DAL.Models;

public class Article : ICommentable
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime Created { get; set; }
    public ICollection<Comment> Comments { get; set; }

    //[ForeignKey("Author")]
    public Guid AuthorId { get; set; }
    public User Author { get; set; }
    public Rubric Rubric { get; set; }
}