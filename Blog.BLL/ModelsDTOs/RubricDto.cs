namespace Blog.BLL.ModelsDTOs;

public class RubricDto : IHierarchical<Guid>
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Name { get; set; }

    public Guid Parent { get; set; }
    public ICollection<RubricDto> Rubrics { get; set; }
}