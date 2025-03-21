namespace Domain.Entities;

[Table("AssignmentTypes")] // Used for Dapper Repository Advanced
public sealed class AssignmentType : BaseEntity
{
    public string? Name { get; set; }

    public static AssignmentType Create(string? name, Guid id = default)
    {
        var type = new AssignmentType
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Name = name,
            Active = true
        };
        
        return type;
    }

    public static void Update(AssignmentType obj, string? name)
    {
        obj.Name = name;
        obj.UpdatedAt = DateTime.UtcNow;
    }

    public static void Remove(AssignmentType obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;
    }
}
