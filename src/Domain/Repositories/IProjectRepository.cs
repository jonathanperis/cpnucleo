namespace Domain.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id);
    Task<PaginatedResult<Project?>> GetAllAsync(PaginationParams pagination, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(Project? entity);
    Task<bool> UpdateAsync(Project? entity);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
