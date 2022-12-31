namespace Cpnucleo.Domain.Common.Repositories.Interfaces;

public interface IGenericRepository<TEntity> : IDisposable
{
    Task<TEntity> AddAsync(TEntity entity);

    void Update(TEntity entity);

    IQueryable<TEntity> Get(Guid id);

    IQueryable<TEntity> All(bool getDependencies = false);

    Task RemoveAsync(Guid id);

    void Detatch(TEntity entity);
}
