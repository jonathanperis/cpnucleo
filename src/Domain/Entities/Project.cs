namespace Domain.Entities;

public sealed class Project : BaseEntity
{
    public string? Name { get; private set; }

    public Ulid SystemId { get; private set; }
    public System? System { get; private set; }

    public static Project Create(string name, Ulid systemId, Ulid id = default)
    {
        return new Project
        {
            Id = id == Ulid.Empty ? Ulid.NewUlid() : id,
            Name = name,
            SystemId = systemId,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static Project Update(Project obj, string name, Ulid systemId)
    {
        obj.Name = name;
        obj.SystemId = systemId;
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
