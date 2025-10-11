namespace GrpcServer.Contracts.Commands.UserAssignment;

/// <summary>
/// Command model for fetching an userAssignment by its unique identifier.
/// </summary>
public class GetUserAssignmentByIdCommand : ICommand<GetUserAssignmentByIdResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the userAssignment.
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// Result model for the userAssignment fetched by its unique identifier.
/// </summary>
public class GetUserAssignmentByIdResult
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
    /// Gets or sets the userAssignment details.
    /// </summary>
    public UserAssignmentDto? UserAssignment { get; set; }
}
