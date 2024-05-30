namespace Domain;

public sealed class Impediment : BaseEntity
{
    public string? Name { get; private set; }

    public static Impediment Create (string name, Ulid id = default)
    {
        return new Impediment
        {   
            Id = id == Ulid.Empty ? Ulid.NewUlid () : id,
            Name = name,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static Impediment Update (Impediment obj, string name)
    {
        obj.Name = name;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }
}