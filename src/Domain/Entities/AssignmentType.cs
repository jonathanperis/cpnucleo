namespace Domain.Entities;

public sealed class AssignmentType : BaseEntity
{
    public string? Name { get; set; }

    public static AssignmentType Create(string? name, Guid id = default)
    {
        var type = new AssignmentType
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow
        };
        type.Name = name;
        type.Active = true;
        return type;
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
