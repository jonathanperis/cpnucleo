namespace Domain.Entities;

public sealed class AssignmentImpediment : BaseEntity
{
    public string? Description { get; private set; }

    public Ulid AssignmentId { get; private set; }
    public Assignment? Assignment { get; private set; }
    public Ulid ImpedimentId { get; private set; }
    public Impediment? Impediment { get; private set; }

    public static AssignmentImpediment Create(string description, Ulid assignmentId, Ulid impedimentId, Ulid id = default)
    {
        return new AssignmentImpediment
        {
            Id = id == Ulid.Empty ? Ulid.NewUlid() : id,
            Description = description,
            AssignmentId = assignmentId,
            ImpedimentId = impedimentId,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
    }

    public static AssignmentImpediment Update(AssignmentImpediment obj, string description, Ulid assignmentId, Ulid impedimentId)
    {
        obj.Description = description;
        obj.AssignmentId = assignmentId;
        obj.ImpedimentId = impedimentId;
        obj.UpdatedAt = DateTime.UtcNow;

        return obj;
    }

    public static AssignmentImpediment Remove(AssignmentImpediment obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;

        return obj;
    }
}
