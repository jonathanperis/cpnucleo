namespace Domain.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id);
    Task<PaginatedResult<Project?>> GetAllAsync(PaginationParams pagination, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(Project? entity);
    Task<bool> UpdateAsync(Project? entity);
    Task<bool> UpdateIfVersionAsync(Project entity, DateTime expectedVersion, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> RemoveManyAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id);
}
