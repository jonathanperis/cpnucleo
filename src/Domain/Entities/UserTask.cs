namespace Domain.Entities;

public sealed class UserTask : BaseEntity
{
    public Ulid UserId { get; private set; }
    public Ulid TaskId { get; private set; }
    public User? User { get; private set; }
    public Task? Task { get; private set; }

    public static UserTask Create(Ulid userId, Ulid taskId, Ulid id = default)
    {
        return new UserTask
        {
            Id = id == Ulid.Empty ? Ulid.NewUlid() : id,
            UserId = userId,
            TaskId = taskId,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static UserTask Update(UserTask obj, Ulid userId, Ulid taskId)
    {
        obj.UserId = userId;
        obj.TaskId = taskId;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }

    public static UserTask Remove(UserTask obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;

        return obj;
    }
}
