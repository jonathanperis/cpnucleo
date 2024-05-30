namespace Domain;

public sealed class Workflow : BaseEntity
{
    public string? Name { get; private set; }
    public byte Order { get; private set; }

    public static Workflow Create (string name, byte order, Ulid id = default)
    {
        return new Workflow
        {   
            Id = id == Ulid.Empty ? Ulid.NewUlid () : id,
            Name = name,
            Order = order,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static Workflow Update (Workflow obj, string name, byte order)
    {
        obj.Name = name;
        obj.Order = order;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }

    public static string GetColumnSize(int columns)
    {
        columns = columns == 1 ? 2 : columns;

        var i = 12 / columns;
        return i.ToString();
    }
}