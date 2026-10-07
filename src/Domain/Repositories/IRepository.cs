namespace Domain.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedResult<T?>> GetAllAsync(PaginationParams pagination, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(T? entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(T? entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateIfVersionAsync(T entity, DateTime expectedVersion, CancellationToken cancellationToken = default);
    /// <summary>Soft-deletes every id or none: returns false (and changes nothing) when any id is missing.</summary>
    Task<bool> RemoveManyAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default);
    /// <summary>
    /// Undoes the soft delete of every id or none: returns false (and changes nothing) when any id is
    /// missing, still active or not visible to the caller.
    /// </summary>
    Task<bool> RestoreManyAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
