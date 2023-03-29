using System.Linq.Expressions;

namespace Blog.DAL.Contracts;

public interface IGenericRepository<TEntity> where TEntity : class
{
    public TEntity GetById(Guid id);
    public IEnumerable<TEntity> GetAll();
    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    public void Add(TEntity entity);
    public void AddRange(IEnumerable<TEntity> entities);
    public void Remove(TEntity entity);
    public void RemoveRange(IEnumerable<TEntity> entity);

    Task<TEntity> GetByIdAsync(Guid id);
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<Task> AddAsync(TEntity entity);
    Task<Task> AddRangeAsync(IEnumerable<TEntity> entities);
}