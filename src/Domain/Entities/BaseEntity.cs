namespace Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected init; }
    public DateTime CreatedAt { get; protected init; }
    public DateTime? UpdatedAt { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public bool Active { get; protected set; }

    protected static Guid GetNewId(Guid id)
    {
        return id == Guid.Empty ? Guid.NewGuid() : id;
    }
}
