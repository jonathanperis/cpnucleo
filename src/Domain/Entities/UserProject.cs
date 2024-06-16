namespace Domain.Entities;

public sealed class UserProject : BaseEntity
{
    public Ulid UserId { get; private set; }
    public Ulid ProjectId { get; private set; }
    public User? User { get; private set; }
    public Project? Project { get; private set; }

    public static UserProject Create(Ulid userId, Ulid projectId, Ulid id = default)
    {
        return new UserProject
        {
            Id = id == Ulid.Empty ? Ulid.NewUlid() : id,
            UserId = userId,
            ProjectId = projectId,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static UserProject Update(UserProject obj, Ulid userId, Ulid projectId)
    {
        obj.UserId = userId;
        obj.ProjectId = projectId;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }

    public static UserProject Remove(UserProject obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;

        return obj;
    }
}
