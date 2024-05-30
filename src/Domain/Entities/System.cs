namespace Domain;

public sealed class System : BaseEntity
{
    public string? Name { get; private set; }
    public string? Description { get; private set; }

    public static System Create (string name, string description, Ulid id = default)
    {
        return new System
        {   
            Id = id == Ulid.Empty ? Ulid.NewUlid () : id,
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static System Update (System obj, string name, string description)
    {
        obj.Name = name;
        obj.Description = description;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }
}