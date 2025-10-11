namespace GrpcServer.Contracts.Commands.AssignmentType;

/// <summary>
/// Command model for fetching an assignmentType by its unique identifier.
/// </summary>
public class GetAssignmentTypeByIdCommand : ICommand<GetAssignmentTypeByIdResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the assignmentType.
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// Result model for the assignmentType fetched by its unique identifier.
/// </summary>
public class GetAssignmentTypeByIdResult
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
    /// Gets or sets the assignmentType details.
    /// </summary>
    public AssignmentTypeDto? AssignmentType { get; set; }
}
