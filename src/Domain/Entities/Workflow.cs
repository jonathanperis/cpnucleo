namespace Domain.Entities;

public sealed class Workflow : BaseEntity
{
    public string? Name { get; set; }
    public byte Order { get; set; }

    public static Workflow Create(string? name, byte order, Guid id = default)
    {
        var workflow = new Workflow
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Name = name,
            Order = order,
            Active = true
        };
        
        return workflow;
    }

    public static Workflow Update(Workflow obj, string? name, byte order)
    {
        obj.Name = name;
        obj.Order = order;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }

    public static Workflow Remove(Workflow obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;

        return obj;
    }

    public static string GetColumnSize(int columns)
    {
        columns = columns == 1 ? 2 : columns;

        var i = 12 / columns;
        return i.ToString();
    }
}
