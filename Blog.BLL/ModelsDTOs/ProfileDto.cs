namespace Blog.BLL.ModelsDTOs;

public class ProfileDto
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Bio { get; set; }
    public string Email { get; set; }
    public string Image { get; set; }
    public bool IsDeleted { get; set; }
}