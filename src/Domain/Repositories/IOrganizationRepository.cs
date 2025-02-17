namespace Domain.Repositories;

public interface IOrganizationRepository
{
    Task<bool> CreateOrganization(Organization organization);
    Task<Organization?> GetOrganizationById(Guid id);
    Task<List<Organization>?> ListOrganization();
    Task<bool> RemoveOrganization(Guid id);
    Task<bool> UpdateOrganization(Guid id, string name, string description);
}
