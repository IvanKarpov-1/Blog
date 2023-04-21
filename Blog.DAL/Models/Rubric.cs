using Blog.DAL.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Blog.DAL.Models;

public class Rubric : IHierarchical<Rubric>
{
    [Key]
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string Name { get; set; }

    public Rubric Parent { get; set; }
    public ICollection<Rubric> Rubrics { get; set; } = new List<Rubric>();
    public ICollection<Article> Articles { get; set; } = new List<Article>();
}