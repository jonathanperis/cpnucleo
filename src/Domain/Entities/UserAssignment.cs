namespace Domain.Entities;

public sealed class UserAssignment : BaseEntity
{
    public Ulid UserId { get; private set; }
    public Ulid AssignmentId { get; private set; }
    public User? User { get; private set; }
    public Assignment? Assignment { get; private set; }

    public static UserAssignment Create(Ulid userId, Ulid assignmentId, Ulid id = default)
    {
        return new UserAssignment
        {
            Id = id == Ulid.Empty ? Ulid.NewUlid() : id,
            UserId = userId,
            AssignmentId = assignmentId,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static UserAssignment Update(UserAssignment obj, Ulid userId, Ulid assignmentId)
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
