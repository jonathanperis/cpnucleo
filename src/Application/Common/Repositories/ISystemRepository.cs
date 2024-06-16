namespace Application.Common.Repositories;

public interface ISystemRepository
{
    Task<bool> CreateSystem(Domain.Entities.System System);
    Task<SystemDto?> GetSystemById(Ulid Id);
    Task<List<SystemDto>?> ListSystem();
    Task<bool> RemoveSystem(Ulid Id);
    Task<bool> UpdateSystem(Ulid id, string name, string description);
}
