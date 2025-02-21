namespace Domain.Entities;

[Table("Impediments")]
public sealed class Impediment : BaseEntity
{
    public string? Name { get; set; }

    public static Impediment Create(string? name, Guid id = default)
    {
        var impediment = new Impediment
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Name = name,
            Active = true
        };
        
        return impediment;
    }

    public static Impediment Update(Impediment obj, string? name)
    {
        obj.Name = name;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }

    public static Impediment Remove(Impediment obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;

        return obj;
    }
}
