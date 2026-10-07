namespace Domain.Entities;

[Table("Workflows")] // Used for Dapper Repository Advanced
public sealed class Workflow : BaseEntity
{
    public string? Name { get; private set; }
    public int Order { get; private set; }

    public static Workflow Create(string? name, int order, Guid id = default)
    {
        return new Workflow
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Name = Guard.Required(name, nameof(Name)),
            Order = Guard.Positive(order, nameof(Order)),
            Active = true
        };
    }

    public static void Update(Workflow obj, string? name, int order)
    {
        obj.Name = Guard.Required(name, nameof(Name));
        obj.Order = Guard.Positive(order, nameof(Order));
        obj.Touch();
    }

    public static void Remove(Workflow obj) => obj.MarkRemoved();

    public static void Restore(Workflow obj) => obj.MarkRestored();
}
