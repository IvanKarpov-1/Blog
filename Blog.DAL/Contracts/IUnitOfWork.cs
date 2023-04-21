namespace Blog.DAL.Contracts;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;
    int Commit();
    Task<int> CommitAsync();
}