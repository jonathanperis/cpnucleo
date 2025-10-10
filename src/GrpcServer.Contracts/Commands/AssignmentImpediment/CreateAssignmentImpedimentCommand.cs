namespace GrpcServer.Contracts.Commands.AssignmentImpediment;

/// <summary>
/// Command model for creating a new assignmentImpediment.
/// </summary>
public class CreateAssignmentImpedimentCommand : ICommand<CreateAssignmentImpedimentResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the assignmentImpediment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the description of the assignmentImpediment.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the assignment.
    /// </summary>
    public Guid AssignmentId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the impediment.
    /// </summary>
    public Guid ImpedimentId { get; set; }
}

/// <summary>
/// Result model for the created assignmentImpediment.
/// </summary>
public class CreateAssignmentImpedimentResult
{
    /// <summary>
    /// Gets or sets the created assignmentImpediment.
    /// </summary>
    public AssignmentImpedimentDto? AssignmentImpediment { get; set; }
}
