namespace Domain.Entities;

public sealed class Assignment : BaseEntity
{
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public byte AmountHours { get; private set; }

    public Ulid ProjectId { get; private set; }
    public Project? Project { get; private set; }
    public Ulid WorkflowId { get; private set; }
    public Workflow? Workflow { get; private set; }
    public Ulid UserId { get; private set; }
    public User? User { get; private set; }
    public Ulid AssignmentTypeId { get; private set; }
    public AssignmentType? AssignmentType { get; private set; }

    public static Assignment Create(string? name,
                               string? description,
                               DateTime startDate,
                               DateTime endDate,
                               byte amountHours,
                               Ulid projectId,
                               Ulid workflowId,
                               Ulid userId,
                               Ulid assignmentTypeId,
                               Ulid id = default)
    {
        return new Assignment
        {
            Id = id == Ulid.Empty ? Ulid.NewUlid() : id,
            Name = name,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            AmountHours = amountHours,
            ProjectId = projectId,
            WorkflowId = workflowId,
            UserId = userId,
            AssignmentTypeId = assignmentTypeId,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static Assignment Update(Assignment obj,
                               string? name,
                               string? description,
                               DateTime startDate,
                               DateTime endDate,
                               byte amountHours,
                               Ulid projectId,
                               Ulid workflowId,
                               Ulid userId,
                               Ulid assignmentTypeId)
    {
        obj.Name = name;
        obj.Description = description;
        obj.StartDate = startDate;
        obj.EndDate = endDate;
        obj.AmountHours = amountHours;
        obj.ProjectId = projectId;
        obj.WorkflowId = workflowId;
        obj.UserId = userId;
        obj.AssignmentTypeId = assignmentTypeId;
        obj.UpdatedAt = DateTime.UtcNow;
        obj.Active = true;

        return obj;
    }

    public static Assignment Remove(Assignment obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;

        return obj;
    }
}
