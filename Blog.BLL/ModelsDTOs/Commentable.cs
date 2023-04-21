namespace Blog.BLL.ModelsDTOs;

public class Commentable
{
    public Guid Id { get; set; }
    public ICollection<CommentDto> Comments { get; set; }
}