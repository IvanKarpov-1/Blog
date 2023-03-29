using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blog.DAL.Contracts;

namespace Blog.DAL.Models;

public class Rubric : IHierarchical<Rubric>
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }


    [ForeignKey("ParentID")]
    public Rubric Parent { get; set; }
    public ICollection<Article> Articles { get; set; }
}