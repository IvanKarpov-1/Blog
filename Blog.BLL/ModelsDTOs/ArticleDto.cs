namespace Blog.BLL.ModelsDTOs;

public class ArticleDto : Commentable
{
    public DateTime CreatedDate { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }

    public ProfileDto Author { get; set; }
    public string RubricName { get; set; }
    public Guid RubricId { get; set; }
}