namespace Domain;

public sealed class Timekeep : BaseEntity
{
    public string? Description { get; private set; }
    public DateTime KeepDate { get; private set; }
    public byte AmountHours { get; private set; }
    public Ulid TaskId { get; private set; }
    public Ulid UserId { get; private set; }
    public Task? Task{ get; private set; }
    public User? User{ get; private set; }

    public static Timekeep Create (string description,
                                   DateTime keepDate,
                                   byte amountHours,
                                   Ulid taskId,
                                   Ulid userId,                                   
                                   Ulid id = default)
    {
        return new Timekeep
        {   
            Id = id == Ulid.Empty ? Ulid.NewUlid () : id,
            Description = description,
            KeepDate = keepDate,
            AmountHours = amountHours,
            TaskId = taskId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static Timekeep Update (Timekeep obj,
                                   string description,
                                   DateTime keepDate,
                                   byte amountHours,
                                   Ulid taskId,
                                   Ulid userId)    
    {
        obj.Description = description;
        obj.KeepDate = keepDate;
        obj.AmountHours = amountHours;
        obj.TaskId = taskId;
        obj.UserId = userId;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }
}