namespace Domain;

public sealed class Task : BaseEntity
{
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public byte AmountHours { get; private set; }
    public Ulid ProjectId { get; private set; }
    public Ulid WorkflowId { get; private set; }
    public Ulid UserId { get; private set; }
    public Ulid TaskTypeId { get; private set; }
    public Project? Project { get; private set; }
    public Workflow? Workflow { get; private set; }
    public User? User { get; private set; }
    public TaskType? TaskType { get; private set; }

    public static Task Create (string name, 
                               string description,
                               DateTime startDate,
                               DateTime endDate,
                               byte amountHours,
                               Ulid projectId,
                               Ulid workflowId,
                               Ulid userId,
                               Ulid taskTypeId,
                               Ulid id = default)
    {
        return new Task
        {   
            Id = id == Ulid.Empty ? Ulid.NewUlid () : id,
            Name = name,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            AmountHours = amountHours,
            ProjectId = projectId,
            WorkflowId = workflowId,
            UserId = userId,
            TaskTypeId = taskTypeId, 
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static Task Update (Task obj,
                               string name, 
                               string description,
                               DateTime startDate,
                               DateTime endDate,
                               byte amountHours,
                               Ulid projectId,
                               Ulid workflowId,
                               Ulid userId,
                               Ulid taskTypeId)
    {
        obj.Name = name;
        obj.Description = description;
        obj.StartDate = startDate;
        obj.EndDate = endDate;
        obj.AmountHours = amountHours;
        obj.ProjectId = projectId;
        obj.WorkflowId = workflowId;
        obj.UserId = userId;
        obj.TaskTypeId = taskTypeId;
        obj.UpdatedAt = DateTime.UtcNow;
        obj.Active = true;

        return obj;
    }
}