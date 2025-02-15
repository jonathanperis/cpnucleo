namespace Domain.Entities;

public abstract class BaseEntity
{
    public Ulid Id { get; protected init; }
    public DateTime CreatedAt { get; protected init; }
    public DateTime? UpdatedAt { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public bool Active { get; protected set; }
}
