namespace Domain.Common.Repositories;

public interface IOrganizationRepository
{
    Task<bool> CreateOrganization(Organization organization);
    Task<OrganizationDto?> GetOrganizationById(Ulid id);
    Task<List<OrganizationDto>?> ListOrganization();
    Task<bool> RemoveOrganization(Ulid id);
    Task<bool> UpdateOrganization(Ulid id, string name, string description);
}
