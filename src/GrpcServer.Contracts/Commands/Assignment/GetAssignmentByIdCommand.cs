namespace GrpcServer.Contracts.Commands.Assignment;

/// <summary>
/// Command model for fetching an assignment by its unique identifier.
/// </summary>
public class GetAssignmentByIdCommand
{
    /// <summary>
    /// Gets or sets the unique identifier for the assignment.
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// Result model for the assignment fetched by its unique identifier.
/// </summary>
public class GetAssignmentByIdResult
{
    /// <summary>
    /// Gets or sets the assignment details.
    /// </summary>
    public AssignmentDto? Assignment { get; set; }
}
