namespace Domain.UoW;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> GetRepository<T>() where T : BaseEntity;
    Task BeginTransactionAsync();
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}