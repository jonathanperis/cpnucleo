namespace Domain.Entities;

[Table("AssignmentImpediments")]
public sealed class AssignmentImpediment : BaseEntity
{
    public string? Description { get; set; }

    public Guid AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }
    public Guid ImpedimentId { get; set; }
    public Impediment? Impediment { get; set; }

    public static AssignmentImpediment Create(string? description, 
                                            Guid assignmentId, 
                                            Guid impedimentId, Guid id = default)
    {
        var impediment = new AssignmentImpediment
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Description = description,
            AssignmentId = assignmentId,
            ImpedimentId = impedimentId,
            Active = true
        };
        
        return impediment;
    }

    public static AssignmentImpediment Update(AssignmentImpediment obj, 
                                        string? description, 
                                        Guid assignmentId, Guid impedimentId)
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
