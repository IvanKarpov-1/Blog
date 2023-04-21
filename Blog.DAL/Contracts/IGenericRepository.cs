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

    public Task<TEntity> GetByIdAsync(Guid id);
    public Task<List<TEntity>> GetAllAsync();
    public Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    public Task<Task> AddAsync(TEntity entity);
    public Task<Task> AddRangeAsync(IEnumerable<TEntity> entities);

    public IQueryable<TEntity> GetQueryable();
}