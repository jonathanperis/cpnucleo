namespace Domain;

public sealed class Project : BaseEntity
{
    public string? Name { get; private set; }
    public Ulid SystemId { get; private set; }
    public System? System { get; private set; }

    public static Project Create (string name, Ulid id = default)
    {
        return new Project
        {   
            Id = id == Ulid.Empty ? Ulid.NewUlid () : id,
            Name = name,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static Project Update (Project obj, string name)
    {
        obj.Name = name;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }
}