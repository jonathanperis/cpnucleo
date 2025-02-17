namespace Domain.Entities;

public sealed class AssignmentType : BaseEntity
{
    public string? Name { get; private set; }

    public static AssignmentType Create(string? name, Ulid id = default)
    {
        return new AssignmentType
        {
            Id = id == Ulid.Empty ? Ulid.NewUlid() : id,
            Name = name,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static AssignmentType Update(AssignmentType obj, string? name)
    {
        obj.Name = name;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }

    public static AssignmentType Remove(AssignmentType obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;

        return obj;
    }
}
