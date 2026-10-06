namespace Domain.Entities;

[Table("Assignments")] // Used for Dapper Repository Advanced
public sealed class Assignment : BaseEntity
{
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int AmountHours { get; private set; }

    public Guid ProjectId { get; private set; }
    public Project? Project { get; private set; }
    public Guid WorkflowId { get; private set; }
    public Workflow? Workflow { get; private set; }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    public Guid AssignmentTypeId { get; private set; }
    public AssignmentType? AssignmentType { get; private set; }

    public static Assignment Create(string? name,
                               string? description,
                               DateTime startDate,
                               DateTime endDate,
                               int amountHours,
                               Guid projectId,
                               Guid workflowId,
                               Guid userId,
                               Guid assignmentTypeId,
                               Guid id = default)
    {
        var assignment = new Assignment
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
        assignment.Apply(name, description, startDate, endDate, amountHours, projectId, workflowId, userId, assignmentTypeId);

        return assignment;
    }

    public static void Update(Assignment obj,
        string? name,
        string? description,
        DateTime startDate,
        DateTime endDate,
        int amountHours,
        Guid projectId,
        Guid workflowId,
        Guid userId,
        Guid assignmentTypeId)
    {
        obj.Apply(name, description, startDate, endDate, amountHours, projectId, workflowId, userId, assignmentTypeId);
        obj.Touch();
    }

    public static void Remove(Assignment obj) => obj.MarkRemoved();

    private void Apply(string? name, string? description, DateTime startDate, DateTime endDate, int amountHours,
        Guid projectId, Guid workflowId, Guid userId, Guid assignmentTypeId)
    {
        var start = Guard.RequiredUtc(startDate, nameof(StartDate));
        var end = Guard.RequiredUtc(endDate, nameof(EndDate));
        if (end < start) throw new DomainException("End date must be on or after start date.", nameof(EndDate));

        Name = Guard.Required(name, nameof(Name));
        Description = Guard.Required(description, nameof(Description), Guard.DescriptionMaxLength);
        StartDate = start;
        EndDate = end;
        AmountHours = Guard.Positive(amountHours, nameof(AmountHours));
        ProjectId = Guard.Reference(projectId, nameof(ProjectId));
        WorkflowId = Guard.Reference(workflowId, nameof(WorkflowId));
        UserId = Guard.Reference(userId, nameof(UserId));
        AssignmentTypeId = Guard.Reference(assignmentTypeId, nameof(AssignmentTypeId));
    }
}
