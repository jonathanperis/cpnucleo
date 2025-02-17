namespace Domain.Entities;

public sealed class Appointment : BaseEntity
{
    public string? Description { get; private set; }
    public DateTime KeepDate { get; private set; }
    public byte AmountHours { get; private set; }

    public Ulid AssignmentId { get; private set; }
    public Assignment? Assignment { get; private set; }
    public Ulid UserId { get; private set; }
    public User? User { get; private set; }

    public static Appointment Create(string? description,
                                   DateTime keepDate,
                                   byte amountHours,
                                   Ulid assignmentId,
                                   Ulid userId,
                                   Ulid id = default)
    {
        return new Appointment
        {
            Id = id == Ulid.Empty ? Ulid.NewUlid() : id,
            Description = description,
            KeepDate = keepDate,
            AmountHours = amountHours,
            AssignmentId = assignmentId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static Appointment Update(Appointment obj,
                                   string? description,
                                   DateTime keepDate,
                                   byte amountHours,
                                   Ulid assignmentId,
                                   Ulid userId)
    {
        obj.Description = description;
        obj.KeepDate = keepDate;
        obj.AmountHours = amountHours;
        obj.AssignmentId = assignmentId;
        obj.UserId = userId;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }

    public static Appointment Remove(Appointment obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;

        return obj;
    }
}
