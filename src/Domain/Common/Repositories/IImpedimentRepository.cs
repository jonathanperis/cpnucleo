namespace Domain.Common.Repositories;

public interface IImpedimentRepository
{
    Task<bool> CreateImpediment(Impediment impediment);
    Task<ImpedimentDto?> GetImpedimentById(Ulid id);
    Task<List<ImpedimentDto>?> ListImpediments();
    Task<bool> UpdateImpediment(Ulid id, string name);
    Task<bool> RemoveImpediment(Ulid id);
}
