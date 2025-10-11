namespace GrpcServer.Contracts.Commands.Assignment;

/// <summary>
/// Command model for fetching an assignment by its unique identifier.
/// </summary>
public class GetAssignmentByIdCommand : ICommand<GetAssignmentByIdResult>
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
    /// Gets or sets a value indicating whether the fetch was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets a message providing additional information about the result.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Gets or sets the assignment details.
    /// </summary>
    public AssignmentDto? Assignment { get; set; }
}
