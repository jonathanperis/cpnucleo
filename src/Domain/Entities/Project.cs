namespace Domain.Entities;

public sealed class Project : BaseEntity
{
    public string? Name { get; private set; }

    public Ulid OrganizationId { get; private set; }
    public Organization? Organization { get; private set; }

    public static Project Create(string name, Ulid organizationId, Ulid id = default)
    {
        return new Project
        {
            Id = id == Ulid.Empty ? Ulid.NewUlid() : id,
            Name = name,
            OrganizationId = organizationId,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static Project Update(Project obj, string name, Ulid organizationId)
    {
        obj.Name = name;
        obj.OrganizationId = organizationId;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }

    public static Project Remove(Project obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;

        return obj;
    }
}
