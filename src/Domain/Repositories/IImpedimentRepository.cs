namespace Domain.Repositories;

public interface IImpedimentRepository
{
    Task<bool> CreateImpediment(Impediment impediment);
    Task<Impediment?> GetImpedimentById(Guid id);
    Task<List<Impediment>?> ListImpediments();
    Task<bool> UpdateImpediment(Guid id, string name);
    Task<bool> RemoveImpediment(Guid id);
}
