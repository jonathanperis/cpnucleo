namespace Domain.Entities;

[Table("Organizations")]
public sealed class Organization : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public static Organization Create(string? name, string? description, Guid id = default)
    {
        var organization = new Organization
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Name = name,
            Description = description,
            Active = true
        };
        
        return organization;
    }

    public static Organization Update(Organization obj, string? name, string? description)
    {
        obj.Name = name;
        obj.Description = description;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }

    public static Organization Remove(Organization obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;

        return obj;
    }
}
