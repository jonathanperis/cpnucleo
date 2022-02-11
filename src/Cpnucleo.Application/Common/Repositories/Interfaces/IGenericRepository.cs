namespace Cpnucleo.Application.Common.Repositories.Interfaces;

public interface IGenericRepository<TEntity> : IDisposable
{
    Task<TEntity> AddAsync(TEntity entity);

    void Update(TEntity entity);

    Task<TEntity> GetAsync(Guid id);

    Task<IEnumerable<TEntity>> AllAsync(bool getDependencies = false);

    Task RemoveAsync(Guid id);

    void Detatch(TEntity entity);
}
