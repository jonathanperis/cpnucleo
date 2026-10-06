namespace Domain.Entities;

[Table("AssignmentTypes")] // Used for Dapper Repository Advanced
public sealed class AssignmentType : BaseEntity
{
    public string? Name { get; private set; }

    public static AssignmentType Create(string? name, Guid id = default)
    {
        return new AssignmentType
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Name = Guard.Required(name, nameof(Name)),
            Active = true
        };
    }

    public static void Update(AssignmentType obj, string? name)
    {
        obj.Name = Guard.Required(name, nameof(Name));
        obj.Touch();
    }

    public static void Remove(AssignmentType obj) => obj.MarkRemoved();
}
