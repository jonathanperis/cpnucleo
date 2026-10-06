namespace Domain.Entities;

[Table("Impediments")] // Used for Dapper Repository Advanced
public sealed class Impediment : BaseEntity
{
    public string? Name { get; private set; }

    public static Impediment Create(string? name, Guid id = default)
    {
        return new Impediment
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Name = Guard.Required(name, nameof(Name)),
            Active = true
        };
    }

    public static void Update(Impediment obj, string? name)
    {
        obj.Name = Guard.Required(name, nameof(Name));
        obj.Touch();
    }

    public static void Remove(Impediment obj) => obj.MarkRemoved();
}
