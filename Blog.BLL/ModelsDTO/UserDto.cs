using Blog.DAL.Models;

namespace Blog.BLL.ModelsDTO;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<Comment> Comments { get; set; }
    public ICollection<Article> Articles { get; set; }
}