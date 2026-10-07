namespace Domain.Entities;

[Table("UserProjects")] // Used for Dapper Repository Advanced
public sealed class UserProject : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid ProjectId { get; private set; }
    public User? User { get; private set; }
    public Project? Project { get; private set; }

    public static UserProject Create(Guid userId, Guid projectId, Guid id = default)
    {
        return new UserProject
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            UserId = Guard.Reference(userId, nameof(UserId)),
            ProjectId = Guard.Reference(projectId, nameof(ProjectId)),
            Active = true
        };
    }

    public static void Update(UserProject obj, Guid userId, Guid projectId)
    {
        obj.UserId = Guard.Reference(userId, nameof(UserId));
        obj.ProjectId = Guard.Reference(projectId, nameof(ProjectId));
        obj.Touch();
    }

    public static void Remove(UserProject obj) => obj.MarkRemoved();

    public static void Restore(UserProject obj) => obj.MarkRestored();
}
