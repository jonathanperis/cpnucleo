namespace Application.Common.Repositories;

public interface IOrganizationRepository
{
    Task<bool> CreateOrganization(Organization Organization);
    Task<OrganizationDto?> GetOrganizationById(Ulid Id);
    Task<List<OrganizationDto>?> ListOrganization();
    Task<bool> RemoveOrganization(Ulid Id);
    Task<bool> UpdateOrganization(Ulid id, string name, string description);
}
