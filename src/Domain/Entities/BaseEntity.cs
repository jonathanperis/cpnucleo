namespace Domain;

public abstract class BaseEntity
{
    public Ulid Id { get; protected set; }
    public long ClusteredKey { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public bool Active { get; protected set; }

    public static BaseEntity Remove (BaseEntity obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;

        return obj;
    }
}