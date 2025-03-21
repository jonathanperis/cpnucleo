namespace Domain.Entities;

[Table("Impediments")] // Used for Dapper Repository Advanced
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

    public static void Update(Impediment obj, string? name)
    {
        obj.Name = name;
        obj.UpdatedAt = DateTime.UtcNow;
    }

    public static void Remove(Impediment obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;
    }
}
