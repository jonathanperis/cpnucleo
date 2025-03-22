namespace Domain.Entities;

[Table("Assignments")] // Used for Dapper Repository Advanced
public sealed class Assignment : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AmountHours { get; set; }

    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    public Guid WorkflowId { get; set; }
    public Workflow? Workflow { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid AssignmentTypeId { get; set; }
    public AssignmentType? AssignmentType { get; set; }

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
            Name = name,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            AmountHours = amountHours,
            ProjectId = projectId,
            WorkflowId = workflowId,
            UserId = userId,
            AssignmentTypeId = assignmentTypeId,
            Active = true
        };
        
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
    }

    public static void Remove(Assignment obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;
    }
}
