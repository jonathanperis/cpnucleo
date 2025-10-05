namespace Domain.Entities;

[Table("Organizations")] // Used for Dapper Repository Advanced
public sealed class Organization : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }

    public static Organization Create(string name, string description, Guid id = default)
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

    public static void Update(Organization obj, string name, string description)
    {
        obj.Name = name;
        obj.Description = description;
        obj.UpdatedAt = DateTime.UtcNow;
    }

    public static void Remove(Organization obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;
    }
}
