using Blog.DAL.Contracts;
using Blog.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Blog.DAL.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly BlogDataContext _blogDbContext;

    public GenericRepository(BlogDataContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public TEntity GetById(Guid id)
    {
        return _blogDbContext.Set<TEntity>().Find(id);
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _blogDbContext.Set<TEntity>().AsEnumerable();
    }

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return _blogDbContext.Set<TEntity>().Where(predicate);
    }

    public void Add(TEntity entity)
    {
        _blogDbContext.Set<TEntity>().Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _blogDbContext.Set<TEntity>().AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        _blogDbContext.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _blogDbContext.Set<TEntity>().RemoveRange(entities);
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await _blogDbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _blogDbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _blogDbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public async Task<Task> AddAsync(TEntity entity)
    {
        await _blogDbContext.Set<TEntity>().AddAsync(entity);
        return Task.CompletedTask;
    }

    public async Task<Task> AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _blogDbContext.Set<TEntity>().AddRangeAsync(entities);
        return Task.CompletedTask;
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return _blogDbContext.Set<TEntity>();
    }
}