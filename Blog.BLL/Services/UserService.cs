using Blog.DAL.Contracts;
using Blog.DAL.Models;

namespace Blog.BLL.Services;

public class UserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<User> Get()
    {
        return _unitOfWork.GetRepository<User>().GetAll();
    }

    public async Task<List<User>> GetAsync()
    {
        return await _unitOfWork.GetRepository<User>().GetAllAsync();
    }

    public async Task<User> GetById(Guid id)
    {
        return await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
    }

    public void Add(User user)
    {
        _unitOfWork.GetRepository<User>().Add(user);
    }
}