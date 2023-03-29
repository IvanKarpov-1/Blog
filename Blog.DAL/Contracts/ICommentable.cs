using Blog.DAL.Models;

namespace Blog.DAL.Contracts;

public interface ICommentable
{
    ICollection<Comment> Comments { get; }
}