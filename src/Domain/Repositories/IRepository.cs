namespace Domain.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<PaginatedResult<T?>> GetAllAsync(PaginationParams pagination);
    Task<Guid> AddAsync(T? entity);
    Task<bool> UpdateAsync(T? entity);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}