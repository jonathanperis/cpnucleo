namespace Domain.Entities;

public sealed class UserAssignment : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid AssignmentId { get; set; }
    public User? User { get; set; }
    public Assignment? Assignment { get; set; }

    public static UserAssignment Create(Guid userId, Guid assignmentId, Guid id = default)
    {
        var assignment = new UserAssignment
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow
        };
        assignment.UserId = userId;
        assignment.AssignmentId = assignmentId;
        assignment.Active = true;
        return assignment;
    }

    public static UserAssignment Update(UserAssignment obj, Guid userId, Guid assignmentId)
    {
        obj.UserId = userId;
        obj.AssignmentId = assignmentId;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }

    public static UserAssignment Remove(UserAssignment obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;

        return obj;
    }
}
