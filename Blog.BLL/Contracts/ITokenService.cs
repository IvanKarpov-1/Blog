using Blog.DAL.Models;

namespace Blog.BLL.Contracts;

public interface ITokenService
{
    string CreateToken(User user);
}