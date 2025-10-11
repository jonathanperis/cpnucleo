namespace GrpcServer.Contracts.Commands.AssignmentImpediment;

/// <summary>
/// Command model for updating an assignmentImpediment.
/// </summary>
public class UpdateAssignmentImpedimentCommand : ICommand<UpdateAssignmentImpedimentResult>
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
/// Result model for the updated assignmentImpediment.
/// </summary>
public class UpdateAssignmentImpedimentResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
}
