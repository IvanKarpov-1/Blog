using Blog.DAL.Contracts;
using Blog.DAL.DataContext;
using Blog.DAL.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Blog.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly BlogDataContext _appDbContext;
    private Dictionary<Type, object> _repositories;
    private bool _disposed;

    public UnitOfWork(BlogDataContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public IGenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
    {
        _repositories ??= new Dictionary<Type, object>();
        
        if (hasCustomRepository)
        {
            var customRepo = _appDbContext.GetService<IGenericRepository<TEntity>>();
            return customRepo;
        }

        var type = typeof(TEntity);
        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = new GenericRepository<TEntity>(_appDbContext);
        }

        return (IGenericRepository<TEntity>)_repositories[type];
    }

    public int Commit()
    {
        return _appDbContext.SaveChanges();
    }

    public async Task<int> CommitAsync()
    {
        return await _appDbContext.SaveChangesAsync();
    }

    public void RollBack()
    {
        _appDbContext.Dispose();
    }

    public async Task RollBackAsync()
    {
        await _appDbContext.DisposeAsync();
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

                _appDbContext.Dispose();
            }
        }

        _disposed = true;
    }
}