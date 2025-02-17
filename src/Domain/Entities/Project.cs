namespace Domain.Entities;

public sealed class Project : BaseEntity
{
    public string? Name { get; set; }

    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }

    public static Project Create(string? name, Guid organizationId, Guid id = default)
    {
        var project = new Project
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Name = name,
            OrganizationId = organizationId,
            Active = true
        };
        
        return project;
    }

    public static Project Update(Project obj, string? name, Guid organizationId)
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
