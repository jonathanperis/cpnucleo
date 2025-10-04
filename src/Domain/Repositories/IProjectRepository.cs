namespace Domain.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id);
    Task<PaginatedResult<Project?>> GetAllAsync(PaginationParams pagination);
    Task<Guid> AddAsync(Project? entity);
    Task<bool> UpdateAsync(Project? entity);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
