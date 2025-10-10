namespace GrpcServer.Contracts.Commands.AssignmentImpediment;

/// <summary>
/// Command model for fetching an assignmentImpediment by its unique identifier.
/// </summary>
public class GetAssignmentImpedimentByIdCommand
{
    /// <summary>
    /// Gets or sets the unique identifier for the assignmentImpediment.
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// Result model for the assignmentImpediment fetched by its unique identifier.
/// </summary>
public class GetAssignmentImpedimentByIdResult
{
    /// <summary>
    /// Gets or sets the assignmentImpediment details.
    /// </summary>
    public AssignmentImpedimentDto? AssignmentImpediment { get; set; }
}
