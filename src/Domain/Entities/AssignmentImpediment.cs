namespace Domain.Entities;

[Table("AssignmentImpediments")] // Used for Dapper Repository Advanced
public sealed class AssignmentImpediment : BaseEntity
{
    public string? Description { get; private set; }

    public Guid AssignmentId { get; private set; }
    public Assignment? Assignment { get; private set; }
    public Guid ImpedimentId { get; private set; }
    public Impediment? Impediment { get; private set; }

    public static AssignmentImpediment Create(string? description,
                                            Guid assignmentId,
                                            Guid impedimentId, Guid id = default)
    {
        var impediment = new AssignmentImpediment
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Active = true
        };
        impediment.Apply(description, assignmentId, impedimentId);

        return impediment;
    }

    public static void Update(AssignmentImpediment obj,
        string? description,
        Guid assignmentId, Guid impedimentId)
    {
        obj.Apply(description, assignmentId, impedimentId);
        obj.Touch();
    }

    public static void Remove(AssignmentImpediment obj) => obj.MarkRemoved();

    private void Apply(string? description, Guid assignmentId, Guid impedimentId)
    {
        Description = Guard.Required(description, nameof(Description), Guard.DescriptionMaxLength);
        AssignmentId = Guard.Reference(assignmentId, nameof(AssignmentId));
        ImpedimentId = Guard.Reference(impedimentId, nameof(ImpedimentId));
    }
}
