namespace Domain.Entities;

[Table("UserAssignments")] // Used for Dapper Repository Advanced
public sealed class UserAssignment : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid AssignmentId { get; private set; }
    public User? User { get; private set; }
    public Assignment? Assignment { get; private set; }

    public static UserAssignment Create(Guid userId, Guid assignmentId, Guid id = default)
    {
        return new UserAssignment
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            UserId = Guard.Reference(userId, nameof(UserId)),
            AssignmentId = Guard.Reference(assignmentId, nameof(AssignmentId)),
            Active = true
        };
    }

    public static void Update(UserAssignment obj, Guid userId, Guid assignmentId)
    {
        obj.UserId = Guard.Reference(userId, nameof(UserId));
        obj.AssignmentId = Guard.Reference(assignmentId, nameof(AssignmentId));
        obj.Touch();
    }

    public static void Remove(UserAssignment obj) => obj.MarkRemoved();

    public static void Restore(UserAssignment obj) => obj.MarkRestored();
}
