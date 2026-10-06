namespace Domain.Entities;

[Table("Appointments")] // Used for Dapper Repository Advanced
public sealed class Appointment : BaseEntity
{
    public string? Description { get; private set; }
    public DateTime KeepDate { get; private set; }
    public int AmountHours { get; private set; }

    public Guid AssignmentId { get; private set; }
    public Assignment? Assignment { get; private set; }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }

    public static Appointment Create(string? description,
                                   DateTime keepDate,
                                   int amountHours,
                                   Guid assignmentId,
                                   Guid userId,
                                   Guid id = default)
    {
        var appointment = new Appointment
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
        appointment.Apply(description, keepDate, amountHours, assignmentId, userId);

        return appointment;
    }

    public static void Update(Appointment obj,
        string? description,
        DateTime keepDate,
        int amountHours,
        Guid assignmentId,
        Guid userId)
    {
        obj.Apply(description, keepDate, amountHours, assignmentId, userId);
        obj.Touch();
    }

    public static void Remove(Appointment obj) => obj.MarkRemoved();

    private void Apply(string? description, DateTime keepDate, int amountHours, Guid assignmentId, Guid userId)
    {
        Description = Guard.Required(description, nameof(Description), Guard.DescriptionMaxLength);
        KeepDate = Guard.RequiredUtc(keepDate, nameof(KeepDate));
        AmountHours = Guard.Positive(amountHours, nameof(AmountHours));
        AssignmentId = Guard.Reference(assignmentId, nameof(AssignmentId));
        UserId = Guard.Reference(userId, nameof(UserId));
    }
}
