namespace Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected init; }
    public DateTime CreatedAt { get; protected init; }
    public DateTime? UpdatedAt { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public bool Active { get; protected set; }

    public static Guid GetNewId(Guid id = default)
    {
        return id == Guid.Empty ? Guid.CreateVersion7() : id;
    }

    protected void Touch() => UpdatedAt = DateTime.UtcNow;

    /// <summary>
    /// Soft-deletes the entity. Removing an already removed entity keeps the original deletion time.
    /// </summary>
    protected void MarkRemoved()
    {
        if (!Active) return;
        Active = false;
        DeletedAt ??= DateTime.UtcNow;
    }

    /// <summary>
    /// Undoes a soft delete. Restoring an active entity changes nothing; nothing but the lifecycle
    /// columns changes, so the row's version (<see cref="UpdatedAt"/>) stays as it was.
    /// </summary>
    protected void MarkRestored()
    {
        if (Active) return;
        Active = true;
        DeletedAt = null;
    }
}
