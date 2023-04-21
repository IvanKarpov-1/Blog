using Blog.DAL.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Blog.DAL.Models;

public class Commentable : ICommentable
{
    [Key]
    public Guid Id { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}