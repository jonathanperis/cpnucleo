namespace Domain.Entities;

public sealed class Organization : BaseEntity
{
    public string? Name { get; private set; }
    public string? Description { get; private set; }

    public static Organization Create(string name, string description, Ulid id = default)
    {
        return new Organization
        {
            Id = id == Ulid.Empty ? Ulid.NewUlid() : id,
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static Organization Update(Organization obj, string name, string description)
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
