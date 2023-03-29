using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blog.DAL.Contracts;

namespace Blog.DAL.Models;

public class Comment : IHierarchical<ICommentable>, ICommentable
{
    [Key]
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }
    public ICollection<Comment> Comments { get; set; }


    //[ForeignKey("Author")]
    public Guid AuthorId { get; set; }
    public User Author { get; set; }
    //[ForeignKey("Parent")]
    [NotMapped]
    public ICommentable Parent { get; set; }
}