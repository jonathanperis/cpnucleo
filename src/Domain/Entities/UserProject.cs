namespace Domain.Entities;

[Table("UserProjects")]
public sealed class UserProject : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public User? User { get; set; }
    public Project? Project { get; set; }

    public static UserProject Create(Guid userId, Guid projectId, Guid id = default)
    {
        var project = new UserProject
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            UserId = userId,
            ProjectId = projectId,
            Active = true
        };
        
        return project;
    }

    public static UserProject Update(UserProject obj, Guid userId, Guid projectId)
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
