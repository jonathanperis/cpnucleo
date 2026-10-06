namespace Domain.UoW;

public interface IUnitOfWork
{
    IRepository<T> GetRepository<T>() where T : BaseEntity;
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
