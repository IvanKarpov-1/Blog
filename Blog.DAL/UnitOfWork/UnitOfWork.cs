using Blog.DAL.Contracts;
using Blog.DAL.DataContext;
using Blog.DAL.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Blog.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly BlogDataContext _blogDbContext;
    private Dictionary<Type, object> _repositories;
    private bool _disposed;

    public UnitOfWork(BlogDataContext blogDbContext)
    {
        _blogDbContext = blogDbContext ?? throw new ArgumentNullException(nameof(blogDbContext));
    }

    public IGenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
    {
        _repositories ??= new Dictionary<Type, object>();
        
        if (hasCustomRepository)
        {
            var customRepo = _blogDbContext.GetService<IGenericRepository<TEntity>>();
            return customRepo;
        }

        var type = typeof(TEntity);
        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = new GenericRepository<TEntity>(_blogDbContext);
        }

        return (IGenericRepository<TEntity>)_repositories[type];
    }

    public int Commit()
    {
        return _blogDbContext.SaveChanges();
    }

    public async Task<int> CommitAsync()
    {
        return await _blogDbContext.SaveChangesAsync();
    }

    public void RollBack()
    {
        _blogDbContext.Dispose();
    }

    public async Task RollBackAsync()
    {
        await _blogDbContext.DisposeAsync();
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _repositories?.Clear();

                _blogDbContext.Dispose();
            }
        }

        _disposed = true;
    }
}