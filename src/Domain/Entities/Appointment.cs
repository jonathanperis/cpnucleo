namespace Domain.Entities;

public sealed class Appointment : BaseEntity
{
    public string? Description { get; set; }
    public DateTime KeepDate { get; set; }
    public byte AmountHours { get; set; }

    public Guid AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public static Appointment Create(string? description,
                                   DateTime keepDate,
                                   byte amountHours,
                                   Guid assignmentId,
                                   Guid userId,
                                   Guid id = default)
    {
        var appointment = new Appointment
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow
        };
        appointment.Description = description;
        appointment.KeepDate = keepDate;
        appointment.AmountHours = amountHours;
        appointment.AssignmentId = assignmentId;
        appointment.UserId = userId;
        appointment.Active = true;
        return appointment;
    }

    public static Appointment Update(Appointment obj,
                                   string? description,
                                   DateTime keepDate,
                                   byte amountHours,
                                   Guid assignmentId,
                                   Guid userId)
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
