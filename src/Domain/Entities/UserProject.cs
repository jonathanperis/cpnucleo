namespace Domain;

public sealed class UserProject : BaseEntity
{
    public Ulid Userid { get; private set; }
    public Ulid ProjectId   { get; private set; }
    public User? User { get; private set; }
    public Project? Project { get; private set; }

    public static UserProject Create (Ulid userId, Ulid projectId, Ulid id = default)
    {
        return new UserProject
        {   
            Id = id == Ulid.Empty ? Ulid.NewUlid () : id,
            Userid = userId,
            ProjectId = projectId,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static UserProject Update (UserProject obj, Ulid userId, Ulid projectId)
    {
        obj.Userid = userId;
        obj.ProjectId = projectId;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }
}