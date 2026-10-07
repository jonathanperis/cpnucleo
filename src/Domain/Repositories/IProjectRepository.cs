namespace Domain.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedResult<Project?>> GetAllAsync(PaginationParams pagination, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(Project? entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Project? entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateIfVersionAsync(Project entity, DateTime expectedVersion, CancellationToken cancellationToken = default);
    Task<bool> RemoveManyAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task<bool> RestoreManyAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
