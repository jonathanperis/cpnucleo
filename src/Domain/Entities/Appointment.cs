namespace Domain.Entities;

public sealed class Appointment : BaseEntity
{
    public string? Description { get; set; }
    public DateTime KeepDate { get; set; }
    public int AmountHours { get; set; }

    public Guid AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }

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
            Description = description,
            KeepDate = keepDate,
            AmountHours = amountHours,
            AssignmentId = assignmentId,
            UserId = userId,
            Active = true
        };
        
        return appointment;
    }

    public static Appointment Update(Appointment obj,
                                   string? description,
                                   DateTime keepDate,
                                   int amountHours,
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
