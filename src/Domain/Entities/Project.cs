namespace Domain.Entities;

[Table("Projects")] // Used for Dapper Repository Advanced
public sealed class Project : BaseEntity
{
    public string? Name { get; private set; }

    public Guid OrganizationId { get; private set; }
    public Organization? Organization { get; private set; }

    public static Project Create(string? name, Guid organizationId, Guid id = default)
    {
        return new Project
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Name = Guard.Required(name, nameof(Name)),
            OrganizationId = Guard.Reference(organizationId, nameof(OrganizationId)),
            Active = true
        };
    }

    public static void Update(Project obj, string? name, Guid organizationId)
    {
        obj.Name = Guard.Required(name, nameof(Name));
        obj.OrganizationId = Guard.Reference(organizationId, nameof(OrganizationId));
        obj.Touch();
    }

    public static void Remove(Project obj) => obj.MarkRemoved();
}
