namespace Domain.Entities;

[Table("Organizations")] // Used for Dapper Repository Advanced
public sealed class Organization : BaseEntity
{
    public string? Name { get; private set; }
    public string? Description { get; private set; }

    public static Organization Create(string? name, string? description, Guid id = default)
    {
        return new Organization
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Name = Guard.Required(name, nameof(Name)),
            Description = Guard.Optional(description, nameof(Description)),
            Active = true
        };
    }

    public static void Update(Organization obj, string? name, string? description)
    {
        obj.Name = Guard.Required(name, nameof(Name));
        obj.Description = Guard.Optional(description, nameof(Description));
        obj.Touch();
    }

    public static void Remove(Organization obj) => obj.MarkRemoved();
}
