namespace Domain;

public sealed class TaskImpediment : BaseEntity
{
    public string? Description { get; private set; }
    public Ulid TaskId { get; private set; }
    public Ulid ImpedimentId { get; private set; }
    public Task? Task { get; private set; }
    public Impediment? Impediment{ get; private set; }

    public static TaskImpediment Create (string description, Ulid taskId, Ulid impedimentId, Ulid id = default)
    {
        return new TaskImpediment
        {   
            Id = id == Ulid.Empty ? Ulid.NewUlid () : id,
            Description = description,
            TaskId = taskId,
            ImpedimentId = impedimentId,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static TaskImpediment Update (TaskImpediment obj, string description, Ulid taskId, Ulid impedimentId)
    {
        obj.Description = description;
        obj.TaskId = taskId;
        obj.ImpedimentId = impedimentId;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }
}