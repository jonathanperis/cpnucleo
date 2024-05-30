namespace Domain;

public sealed class TaskType : BaseEntity
{
    public string? Name { get; private set; }

    public static TaskType Create (string name, Ulid id = default)
    {
        return new TaskType
        {   
            Id = id == Ulid.Empty ? Ulid.NewUlid () : id,
            Name = name,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static TaskType Update (TaskType obj, string name)
    {
        obj.Name = name;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }
}