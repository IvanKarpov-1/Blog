namespace Blog.BLL.ModelsDTOs;

public class CommentDto : Commentable
{
    public DateTime CreatedDate { get; set; }
    public string Content { get; set; }
    
    public ProfileDto Author { get; set; }
    public Guid ParentId { get; set; }
}